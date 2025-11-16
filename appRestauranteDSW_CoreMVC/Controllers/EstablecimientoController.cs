using appRestauranteDSW_CoreMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace appRestauranteDSW_CoreMVC.Controllers
{
    public class EstablecimientoController : Controller
    {
        private readonly HttpClient _httpClient;

        public EstablecimientoController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        // GET: Establecimiento/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var url = $"https://localhost:7296/api/Establecimiento/{id}";
            var establecimiento = await _httpClient.GetFromJsonAsync<Establecimiento>(url);

            if (establecimiento == null)
                return NotFound();

            return View(establecimiento);
        }

        // POST: Establecimiento/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(Establecimiento est)
        {
            if (!ModelState.IsValid)
                return View(est);

            var url = $"https://localhost:7296/api/Establecimiento/{est.Id}";
            var response = await _httpClient.PutAsJsonAsync(url, est);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Establecimiento actualizado correctamente";
                return RedirectToAction("Edit", new { id = est.Id });
            }
            else
            {
                TempData["Error"] = "Error al actualizar el establecimiento";
                return View(est);
            }
        }
    }
}