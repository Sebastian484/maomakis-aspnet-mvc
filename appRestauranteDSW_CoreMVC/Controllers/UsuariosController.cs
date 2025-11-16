using appRestauranteDSW_CoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace appRestauranteDSW_CoreMVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly string _baseUrl = "https://localhost:7296/api/Usuarios";

        public async Task<IActionResult> Index(string filtro)
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                HttpResponseMessage response = await client.GetAsync(""); // GET api/usuarios
                string apiResponse = await response.Content.ReadAsStringAsync();
                usuarios = JsonConvert.DeserializeObject<List<Usuario>>(apiResponse).ToList();
            }

            // Si hay filtro, aplica búsqueda local
            if (!string.IsNullOrEmpty(filtro))
            {
                filtro = filtro.ToLower();
                usuarios = usuarios
                    .Where(u => (u.codigo != null && u.codigo.ToString().Contains(filtro))
                             || (!string.IsNullOrEmpty(u.correo) && u.correo.ToLower().Contains(filtro)))
                    .ToList();
            }

            return View(usuarios);
        }

        [HttpPost]
        public async Task<IActionResult> Desactivar(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                HttpResponseMessage response = await client.PutAsync($"{id}/desactivar", null);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = $"No se pudo desactivar el usuario {id}";
                }
                else
                {
                    TempData["Success"] = $"Usuario {id} desactivado correctamente";
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }

}
