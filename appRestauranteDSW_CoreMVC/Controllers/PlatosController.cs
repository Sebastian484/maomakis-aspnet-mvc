using appRestauranteDSW_CoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace appRestauranteDSW_CoreMVC.Controllers
{
    public class PlatosController : Controller
    {
        private readonly string _baseUrl = "https://localhost:7296/api/Platos";
        private readonly string _categoriasUrl = "https://localhost:7296/api/Categorias";

        // GET: Platos
        public async Task<IActionResult> Index()
        {
            List<Plato> platos = new();
            List<CategoriaPlato> categorias = new();

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(_baseUrl);
                string apiResponse = await response.Content.ReadAsStringAsync();
                platos = JsonConvert.DeserializeObject<List<Plato>>(apiResponse);

                // Traer categorías
                HttpResponseMessage responseCategorias = await client.GetAsync(_categoriasUrl);
                string apiResponseCategorias = await responseCategorias.Content.ReadAsStringAsync();
                categorias = JsonConvert.DeserializeObject<List<CategoriaPlato>>(apiResponseCategorias);
            }

            // Pasamos las categorías en un diccionario para fácil acceso en la vista
            ViewBag.CategoriasDic = categorias.ToDictionary(c => c.id, c => c.nombre);

            return View(platos);
        }

        // GET: Platos/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categorias = await GetCategorias();
            return View();
        }

        // POST: Platos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plato plato, IFormFile ImagenFile)
        {
            if (ModelState.IsValid)
            {
                if (ImagenFile != null && ImagenFile.Length > 0)
                {
                    // Nombre del archivo = nombre del plato + extensión
                    var fileName = plato.nombre.Replace(" ", "_") + Path.GetExtension(ImagenFile.FileName);

                    // Ruta física en wwwroot/images/
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImagenFile.CopyToAsync(stream);
                    }

                    // Guardar la ruta relativa en el modelo
                    plato.imagen = "/images/" + fileName;
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7296/api/Platos");

                    var content = new StringContent(JsonConvert.SerializeObject(plato), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("", content);

                    if (response.IsSuccessStatusCode)
                        return RedirectToAction(nameof(Index));
                }
            }

            return View(plato);
        }

        // GET: Platos/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            Plato plato = null;
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"{_baseUrl}/{id}");
                string apiResponse = await response.Content.ReadAsStringAsync();
                plato = JsonConvert.DeserializeObject<Plato>(apiResponse);
            }

            ViewBag.Categorias = await GetCategorias();
            return View(plato);
        }

        // POST: Platos/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(string id, Plato plato)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = await GetCategorias();
                return View(plato);
            }

            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(plato), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync($"{_baseUrl}/{id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "No se pudo actualizar el plato.";
                    ViewBag.Categorias = await GetCategorias();
                    return View(plato);
                }
            }

            TempData["Success"] = "Plato actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Platos/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            Plato plato = null;
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"{_baseUrl}/{id}");
                string apiResponse = await response.Content.ReadAsStringAsync();
                plato = JsonConvert.DeserializeObject<Plato>(apiResponse);
            }
            return View(plato);
        }

        // POST: Platos/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync($"{_baseUrl}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "No se pudo eliminar el plato.";
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["Success"] = "Plato eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Platos/Details/{id}
        public async Task<IActionResult> Details(string id)
        {
            Plato plato = null;
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"{_baseUrl}/{id}");
                string apiResponse = await response.Content.ReadAsStringAsync();
                plato = JsonConvert.DeserializeObject<Plato>(apiResponse);
            }

            // Buscar el nombre de la categoría
            var categorias = await GetCategorias();
            var categoria = categorias.FirstOrDefault(c => c.id == plato.categoria_plato_id);

            ViewBag.CategoriaNombre = categoria != null ? categoria.nombre : "Sin categoría";

            return View(plato);
        }


        // Método auxiliar para traer categorías
        private async Task<List<CategoriaPlato>> GetCategorias()
        {
            List<CategoriaPlato> categorias = new();
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(_categoriasUrl);
                string apiResponse = await response.Content.ReadAsStringAsync();
                categorias = JsonConvert.DeserializeObject<List<CategoriaPlato>>(apiResponse);
            }
            return categorias;
        }
    }
}

