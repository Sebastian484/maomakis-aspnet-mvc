using appRestauranteDSW_CoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace appRestauranteDSW_CoreMVC.Controllers
{
    public class ComandaController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrlMesas = "https://localhost:7296/api/Mesas";
        private readonly string _baseUrlComandas = "https://localhost:7296/api/Comandas";

        public ComandaController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        // 🔹 Listado de mesas
        public async Task<IActionResult> Index()
        {
            List<Mesas> mesas = new();

            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrlMesas);
            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                mesas = JsonConvert.DeserializeObject<List<Mesas>>(apiResponse);
            }

            return View(mesas);
        }

        private readonly string _baseUrlCategorias = "https://localhost:7296/api/Categorias";
        // GET: Comanda/Create
        public async Task<IActionResult> Create(int mesaId)
        {
          

            // Traer información de la mesa
            Mesas mesa = null;
            HttpResponseMessage respMesa = await _httpClient.GetAsync($"{_baseUrlMesas}/{mesaId}");
            if (respMesa.IsSuccessStatusCode)
            {
                string jsonMesa = await respMesa.Content.ReadAsStringAsync();
                mesa = JsonConvert.DeserializeObject<Mesas>(jsonMesa);
            }

            // Modelo inicial
            var model = new Comanda
            {
                MesaId = mesaId,
                FechaEmision = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                EmpleadoId = HttpContext.Session.GetInt32("EmpleadoId"),
                CantidadAsientos = mesa?.cantidad_asientos
            };

            // Traer categorías
            List<CategoriaPlato> categorias = new();
            HttpResponseMessage respCategorias = await _httpClient.GetAsync(_baseUrlCategorias);
            if (respCategorias.IsSuccessStatusCode)
            {
                string json = await respCategorias.Content.ReadAsStringAsync();
                categorias = JsonConvert.DeserializeObject<List<CategoriaPlato>>(json);
            }

            ViewBag.Categorias = new SelectList(categorias, "id", "nombre");
            return View(model);
        }

        private readonly string _baseUrlDetalle = "https://localhost:7296/api/DetalleComanda";

        // POST: Comanda/Create
        [HttpPost]
        public async Task<IActionResult> Create(Comanda comanda)
        {
            if (!ModelState.IsValid)
                return View(comanda);

            // 📌 Enviar solo lo que la API espera
            var comandaPayload = new
            {
                cantidad_asientos = comanda.CantidadAsientos,
                fecha_emision = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                precio_total = comanda.PrecioTotal ?? 0,
                empleado_id = comanda.EmpleadoId,
                estado_comanda_id = comanda.EstadoComandaId ?? 2, // En Proceso
                mesa_id = comanda.MesaId
            };

            StringContent contentComanda = new StringContent(
                JsonConvert.SerializeObject(comandaPayload),
                Encoding.UTF8,
                "application/json"
            );

            HttpResponseMessage respComanda = await _httpClient.PostAsync(_baseUrlComandas, contentComanda);

            string respuestaApi = await respComanda.Content.ReadAsStringAsync();
            Console.WriteLine("📌 Respuesta API Comanda: " + respuestaApi);

            if (!respComanda.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error al crear la comanda: " + respuestaApi);
                return View(comanda);
            }

            // 📌 La API te devuelve un envoltorio { message, data }
            var responseObj = JsonConvert.DeserializeObject<dynamic>(respuestaApi);
            int createdComandaId = (int)responseObj.data.id;

            // 📌 Serializar y enviar cada detalle con los nombres que espera la API
            foreach (var detalle in comanda.Detalles)
            {
                var detallePayload = new
                {
                    comanda_id = createdComandaId,
                    plato_id = detalle.PlatoId,
                    cantidad_pedido = detalle.Cantidad,
                    precio_unitario = detalle.PrecioUnitario,
                    observacion = detalle.Observacion
                };

                StringContent contentDetalle = new StringContent(
                    JsonConvert.SerializeObject(detallePayload),
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage respDetalle = await _httpClient.PostAsync(_baseUrlDetalle, contentDetalle);

                string respDetalleStr = await respDetalle.Content.ReadAsStringAsync();
                Console.WriteLine("📌 Respuesta API Detalle: " + respDetalleStr);

                if (!respDetalle.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", "Error al crear detalle: " + respDetalleStr);
                    return View(comanda);
                }
            }

            TempData["Success"] = "Comanda y detalles creados correctamente";
            return RedirectToAction("Index", "Comanda");
        }


        // Método auxiliar para cargar platos por categoría (opcional si quieres hacerlo desde API)
        public async Task<JsonResult> GetPlatosPorCategoria(int categoriaId)
        {
            HttpResponseMessage resp = await _httpClient.GetAsync($"https://localhost:7296/api/Platos/categoria/{categoriaId}");
            if (!resp.IsSuccessStatusCode) return Json(new List<Plato>());

            string json = await resp.Content.ReadAsStringAsync();
            var platos = JsonConvert.DeserializeObject<List<Plato>>(json);
            return Json(platos);
        }

        //PARA EL COCINERO *************************

        // GET: Comanda/Pendientes
        public async Task<IActionResult> Pendientes()
        {
            List<Comanda> comandas = new();

            // Traemos todas las comandas desde la API
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrlComandas);
            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                comandas = JsonConvert.DeserializeObject<List<Comanda>>(apiResponse);

                Console.WriteLine(apiResponse); // Para ver lo que viene de la API

                foreach (var c in comandas)
                {
                    Console.WriteLine($"Comanda {c.Id} - Estado: {c.EstadoComandaId}");
                }

            }



            // Filtrar solo las pendientes (estado 1)
            var pendientes = comandas.Where(c => c.EstadoComandaId == 1).ToList();

            return View(pendientes);
        }

        // GET: Comanda/Detalles/5
        public async Task<IActionResult> Detalles(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrlComandas}/{id}/detalles");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "No se pudieron cargar los detalles.";
                return RedirectToAction("Pendientes");
            }

            var apiResponse = await response.Content.ReadAsStringAsync();
            var detalles = JsonConvert.DeserializeObject<List<DetalleComandaViewModel>>(apiResponse);

            ViewBag.ComandaId = id;
            return View(detalles); // 👉 ahora va a la vista completa Detalles.cshtml
        }



        // POST: Comanda/MarcarPreparado/5
        [HttpPost]
        public async Task<IActionResult> MarcarPreparado(int id)
        {
            StringContent content = new StringContent("2", Encoding.UTF8, "application/json"); // Estado 2 = Preparado
            HttpResponseMessage response = await _httpClient.PutAsync($"{_baseUrlComandas}/{id}/estado", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = $"Comanda #{id} marcada como Preparada";
            }
            else
            {
                TempData["Error"] = $"No se pudo actualizar la comanda #{id}";
            }

            return RedirectToAction("Pendientes");
        }

        // PARA CAJERO FACTURAR
        // GET: Comanda/Preparadas
        public async Task<IActionResult> Preparadas()
        {
            List<Comanda> comandas = new();

            // Obtener Comandas
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrlComandas);
            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                comandas = JsonConvert.DeserializeObject<List<Comanda>>(apiResponse);
            }

            // Filtrar solo las preparadas (estado 2)
            var preparadas = comandas.Where(c => c.EstadoComandaId == 2).ToList();

            return View(preparadas);
        }




    }
}
