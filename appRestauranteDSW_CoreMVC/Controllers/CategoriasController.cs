using appRestauranteDSW_CoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace appRestauranteDSW_CoreMVC.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly string _baseUrl = "https://localhost:7296/api/Categorias";

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            List<CategoriaPlato> categorias = new();

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(_baseUrl);
                string apiResponse = await response.Content.ReadAsStringAsync();
                categorias = JsonConvert.DeserializeObject<List<CategoriaPlato>>(apiResponse);
            }

            return View(categorias);
        }

        // GET: Categorias/Create
        public IActionResult Create() => View();

        // POST: Categorias/Create
        [HttpPost]
        public async Task<IActionResult> Create(CategoriaPlato categoria)
        {
            if (!ModelState.IsValid) return View(categoria);

            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(categoria), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_baseUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "No se pudo crear la categoría.";
                    return View(categoria);
                }
            }

            TempData["Success"] = "Categoría creada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Categorias/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            CategoriaPlato categoria = null;
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"{_baseUrl}/{id}");
                string apiResponse = await response.Content.ReadAsStringAsync();
                categoria = JsonConvert.DeserializeObject<CategoriaPlato>(apiResponse);
            }
            return View(categoria);
        }

        // POST: Categorias/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(string id, CategoriaPlato categoria)
        {
            if (!ModelState.IsValid) return View(categoria);

            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(categoria), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync($"{_baseUrl}/{id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "No se pudo actualizar la categoría.";
                    return View(categoria);
                }
            }

            TempData["Success"] = "Categoría actualizada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Categorias/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            CategoriaPlato categoria = null;
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"{_baseUrl}/{id}");
                string apiResponse = await response.Content.ReadAsStringAsync();
                categoria = JsonConvert.DeserializeObject<CategoriaPlato>(apiResponse);
            }
            return View(categoria);
        }

        // POST: Categorias/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync($"{_baseUrl}/{id}");
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = JsonConvert.DeserializeObject<dynamic>(result).message.ToString();
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["Success"] = "Categoría eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}

