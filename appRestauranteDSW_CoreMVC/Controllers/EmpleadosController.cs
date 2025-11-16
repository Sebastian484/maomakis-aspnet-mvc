using appRestauranteDSW_CoreMVC.DTOs;
using appRestauranteDSW_CoreMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace appRestauranteDSW_CoreMVC.Controllers
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        private readonly string _baseUrl = "https://localhost:7296/api/Empleados";

        // GET: Empleados
        public async Task<IActionResult> Index(string filtro)
        {
            List<Empleado> empleados = new List<Empleado>();

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(_baseUrl);
                string apiResponse = await response.Content.ReadAsStringAsync();
                empleados = JsonConvert.DeserializeObject<List<Empleado>>(apiResponse).ToList();
            }

            // Búsqueda local por nombre, apellido o DNI
            if (!string.IsNullOrEmpty(filtro))
            {
                filtro = filtro.ToLower();
                empleados = empleados.Where(e =>
                        (!string.IsNullOrEmpty(e.nombre) && e.nombre.ToLower().Contains(filtro)) ||
                        (!string.IsNullOrEmpty(e.apellido) && e.apellido.ToLower().Contains(filtro)) ||
                        (!string.IsNullOrEmpty(e.dni) && e.dni.ToLower().Contains(filtro))
                    ).ToList();
            }

            return View(empleados);
        }

        // GET: Empleados/Create
        public async Task<IActionResult> Create()
        {
            // Consumir API de cargos
            List<Cargo> cargos = new List<Cargo>();
            List<Usuario> usuarios = new List<Usuario>();

            using (var client = new HttpClient())
            {
                // Traer cargos
                var responseCargos = await client.GetAsync("https://localhost:7296/api/Cargos");
                if (responseCargos.IsSuccessStatusCode)
                {
                    string apiResponse = await responseCargos.Content.ReadAsStringAsync();
                    cargos = JsonConvert.DeserializeObject<List<Cargo>>(apiResponse).ToList();
                }

                // Traer usuarios
                var responseUsuarios = await client.GetAsync("https://localhost:7296/api/Usuarios");
                if (responseUsuarios.IsSuccessStatusCode)
                {
                    string apiResponse = await responseUsuarios.Content.ReadAsStringAsync();
                    usuarios = JsonConvert.DeserializeObject<List<Usuario>>(apiResponse).ToList();
                }
            }

            ViewBag.Cargos = new SelectList(cargos, "id", "nombre");
            ViewBag.Usuarios = new SelectList(usuarios, "id", "correo");

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(EmpleadoRegisterDTO model)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7296/api/");

            // 1️⃣ Crear el usuario primero
            var registerRequest = new
            {
                Correo = model.correo,
                Contrasena = model.contrasena
            };

            var registerContent = new StringContent(
                JsonConvert.SerializeObject(registerRequest),
                Encoding.UTF8,
                "application/json"
            );

            HttpResponseMessage registerResponse = await client.PostAsync("Auth/register", registerContent);
            string registerApiResponse = await registerResponse.Content.ReadAsStringAsync();

            if (!registerResponse.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", $"Error creando usuario: {registerApiResponse}");
                return View(model);
            }

            // 2️⃣ Obtener el id del usuario creado
            var registerResult = JsonConvert.DeserializeObject<RegisterResponse>(registerApiResponse);
            int usuarioId = registerResult.Id;

            // 3️⃣ Crear el empleado usando el id del usuario
            var empleadoDto = new
            {
                nombre = model.nombre,
                apellido = model.apellido,
                dni = model.dni,
                telefono = model.telefono,
                cargo_id = model.cargo_id,
                usuario_id = usuarioId
            };

            var empleadoContent = new StringContent(
                JsonConvert.SerializeObject(empleadoDto),
                Encoding.UTF8,
                "application/json"
            );

            HttpResponseMessage empleadoResponse = await client.PostAsync("Empleados", empleadoContent);
            string empleadoApiResponse = await empleadoResponse.Content.ReadAsStringAsync();

            if (!empleadoResponse.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", $"Error creando empleado: {empleadoApiResponse}");
                return View(model);
            }

            TempData["mensaje"] = "Empleado y usuario creados correctamente";
            return RedirectToAction(nameof(Index));
        }






        // GET: Empleados/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Empleado empleado = new Empleado();
            List<Cargo> cargos = new List<Cargo>();
            List<Usuario> usuarios = new List<Usuario>();

            using (var client = new HttpClient())
            {
                // Obtener empleado
                HttpResponseMessage response = await client.GetAsync($"{_baseUrl}/{id}");
                string apiResponse = await response.Content.ReadAsStringAsync();
                empleado = JsonConvert.DeserializeObject<Empleado>(apiResponse);

                // Obtener cargos
                var responseCargos = await client.GetAsync("https://localhost:7296/api/Cargos");
                if (responseCargos.IsSuccessStatusCode)
                {
                    string apiResponseCargos = await responseCargos.Content.ReadAsStringAsync();
                    cargos = JsonConvert.DeserializeObject<List<Cargo>>(apiResponseCargos).ToList();
                }

                // Obtener usuarios
                var responseUsuarios = await client.GetAsync("https://localhost:7296/api/Usuarios");
                if (responseUsuarios.IsSuccessStatusCode)
                {
                    string apiResponseUsuarios = await responseUsuarios.Content.ReadAsStringAsync();
                    usuarios = JsonConvert.DeserializeObject<List<Usuario>>(apiResponseUsuarios).ToList();
                }
            }

            // Pasar a la vista para los dropdowns
            ViewBag.Cargos = new SelectList(cargos, "id", "nombre", empleado.cargo_id);
            ViewBag.Usuarios = new SelectList(usuarios, "id", "correo", empleado.usuario_id);

            return View(empleado);
        }


        // POST: Empleados/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Empleado empleado)
        {
            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(empleado), Encoding.UTF8, "application/json");
                await client.PutAsync($"{_baseUrl}/{id}", content);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Empleados/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Empleado empleado = new Empleado();

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"{_baseUrl}/{id}");
                string apiResponse = await response.Content.ReadAsStringAsync();
                empleado = JsonConvert.DeserializeObject<Empleado>(apiResponse);
            }

            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                await client.DeleteAsync($"{_baseUrl}/{id}");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

