using appRestauranteDSW_CoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace appRestauranteDSW_CoreMVC.Controllers
{
    public class MesasController : Controller
    {

        //ORIGINAL SEBASTIANN


        // GET: Mesas
        public async Task<IActionResult> Index()
        {
            List<Mesas> temporal = new List<Mesas>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7296/api/Mesas");
                HttpResponseMessage response = await client.GetAsync("");
                string apiResponse = await response.Content.ReadAsStringAsync();
                temporal = JsonConvert.DeserializeObject<List<Mesas>>(apiResponse).ToList();
            }
            return View(await Task.Run(() => temporal));
        }

        //CREAR
        public async Task<IActionResult> Create()
        {
            return View(new Mesas());
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Mesas mesa)
        {
            if (!ModelState.IsValid)
                return View(mesa);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7296/api/Mesas");
                StringContent content = new StringContent(
                    JsonConvert.SerializeObject(new { cantidad_asientos = mesa.cantidad_asientos, estado = mesa.estado }),
                    Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    ViewBag.mensaje = $"Error al crear mesa: {response.StatusCode} - {error}";
                    return View(mesa);
                }
            }
        }

        //Editar 
        public async Task<IActionResult> Edit(int id)
        {
            Mesas mesa = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7296/api/Mesas/");
                HttpResponseMessage response = await client.GetAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    mesa = JsonConvert.DeserializeObject<Mesas>(apiResponse);
                }
                else
                {
                    ViewData["ErrorMessage"] = "Mesa no encontrada";
                    return View("Error");
                }
            }
            return View(mesa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Mesas mesa)
        {
            string mensaje = "";

            if (mesa.id == null)
            {
                ViewData["ErrorMessage"] = "El ID de la mesa no es válido.";
                return View("Error");
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7296/api/Mesas/");
                StringContent content = new StringContent(JsonConvert.SerializeObject(mesa), Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PutAsync($"{mesa.id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        mensaje = "Mesa actualizada exitosamente";
                    }
                    else
                    {
                        mensaje = $"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                    }
                }
                catch (Exception ex)
                {
                    mensaje = $"Error al conectar con la API: {ex.Message}";
                }
            }
            ViewBag.mensaje = mensaje;
            return RedirectToAction(nameof(Index));
        }


        //DETALLES
        public async Task<IActionResult> Details(int id)
        {
            Mesas mesa = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7296/api/Mesas/");
                HttpResponseMessage response = await client.GetAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    mesa = JsonConvert.DeserializeObject<Mesas>(apiResponse);
                }
                else
                {
                    ViewData["ErrorMessage"] = "Mesa no encontrada";
                    return View("Error");
                }
            }
            return View(mesa);  // Pasar el modelo de mesa a la vista
        }

        //DELETE/BORRAR
        public async Task<IActionResult> Delete(int id)
        {
            Mesas mesa = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7296/api/Mesas/");
                HttpResponseMessage response = await client.GetAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    mesa = JsonConvert.DeserializeObject<Mesas>(apiResponse);
                }
                else
                {
                    ViewBag.Error = "Mesa no encontrada";
                    return View("Error");
                }
            }

            return View(mesa);  // Retorna la vista con los detalles de la mesa.
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string mensaje = "";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7296/api/Mesas/");
                try
                {
                    HttpResponseMessage response = await client.DeleteAsync($"{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        mensaje = "Mesa eliminada exitosamente";
                    }
                    else
                    {
                        mensaje = $"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                    }
                }
                catch (Exception ex)
                {
                    mensaje = $"Error al conectar con la API: {ex.Message}";
                }
            }
            ViewBag.mensaje = mensaje;
            return RedirectToAction(nameof(Index));
        }



    }
}
