using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

public class RegisterController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RegisterController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(); // Vista con formulario de registro
    }

    [HttpPost]
    public async Task<IActionResult> Index(string correo, string contrasena)
    {
        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri("https://localhost:7296");

        var registerRequest = new
        {
            correo = correo,
            contrasena = contrasena
        };

        var content = new StringContent(JsonConvert.SerializeObject(registerRequest), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/Auth/register", content);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RegisterResponse>(json);

            ViewBag.Success = result.Message;
            return View();
        }

        ViewBag.Error = "No se pudo registrar el usuario";
        return View();
    }
}

public class RegisterResponse
{
    public string Message { get; set; }
    public int Id { get; set; }
}
