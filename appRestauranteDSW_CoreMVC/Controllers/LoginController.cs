using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

public class LoginController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public LoginController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(); // Vista con formulario de login
    }

    [HttpPost]
    public async Task<IActionResult> Index(string correo, string contrasena)
    {
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri("https://localhost:7296"); // URL de tu API

        var loginRequest = new
        {
            correo = correo,        
            contrasena = contrasena  
        };

        var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/Auth/login", content);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<LoginResponse>(json);

            // Crear claims
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, result.Usuario),
        new Claim(ClaimTypes.Role, result.Rol),
        new Claim("EmpleadoId", result.EmpleadoId.ToString())
    };

            var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");

            // Iniciar sesión con cookie
            await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));

            // Opcional: mantener datos en session
            HttpContext.Session.SetInt32("EmpleadoId", result.EmpleadoId);
            HttpContext.Session.SetString("JWToken", result.Token);
            HttpContext.Session.SetString("EmpleadoRol", result.Rol);

            return RedirectToAction("Index", "Home");
        }


        ViewBag.Error = "Credenciales inválidas";
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        // Borra la cookie de autenticación
        await HttpContext.SignOutAsync("MyCookieAuth");

        // Opcional: limpiar la sesión
        HttpContext.Session.Clear();

        // Redirigir al login
        return RedirectToAction("Index", "Login");
    }
}

public class LoginResponse
{
    public string Token { get; set; }
    public string Usuario { get; set; }
    public string Rol { get; set; }
    public int EmpleadoId { get; set; }
    public DateTime Expira { get; set; }
}
