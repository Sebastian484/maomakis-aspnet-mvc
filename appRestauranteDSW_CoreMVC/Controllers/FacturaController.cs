using appRestauranteDSW_CoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

public class FacturaController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrlComandas = "https://localhost:7296/api/Comandas";
    private readonly string _baseUrlTipoComprobante = "https://localhost:7296/api/TipoComprobante";
    private readonly string _baseUrlComprobante = "https://localhost:7296/api/Comprobantes";

    public async Task<IActionResult> Index()
    {
        var comprobantes = new List<Comprobante>();

        HttpResponseMessage resp = await _httpClient.GetAsync(_baseUrlComprobante);
        if (resp.IsSuccessStatusCode)
        {
            string apiResponse = await resp.Content.ReadAsStringAsync();
            comprobantes = JsonConvert.DeserializeObject<List<Comprobante>>(apiResponse);
        }

        return View(comprobantes);
    }

    public FacturaController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<IActionResult> Nuevo(int comandaId)
    {
        var vm = new FacturaViewModel();

        // 1. Obtener la comanda
        HttpResponseMessage respComanda = await _httpClient.GetAsync($"{_baseUrlComandas}/{comandaId}");
        if (respComanda.IsSuccessStatusCode)
        {
            string apiResponse = await respComanda.Content.ReadAsStringAsync();
            vm.Comanda = JsonConvert.DeserializeObject<Comanda>(apiResponse);
        }

        // 2. Obtener los tipos de comprobante
        HttpResponseMessage respTipos = await _httpClient.GetAsync(_baseUrlTipoComprobante);
        if (respTipos.IsSuccessStatusCode)
        {
            string apiResponse = await respTipos.Content.ReadAsStringAsync();
            vm.TiposComprobante = JsonConvert.DeserializeObject<List<TipoComprobante>>(apiResponse);
        }

        // 3. Calcular subtotal e IGV
        var total = vm.Comanda?.PrecioTotal ?? 0;
        var subTotal = Math.Round(total / 1.18m, 2);
        var igv = Math.Round(total - subTotal, 2);

        vm.Comprobante = new Comprobante
        {
            ComandaId = vm.Comanda.Id,
            EmpleadoId = HttpContext.Session.GetInt32("EmpleadoId"),
            PrecioTotalPedido = total,
            SubTotal = subTotal,
            IgvTotal = igv,
            DescuentoTotal = 0
        };

        // 4. Obtener los detalles de la comanda
        HttpResponseMessage respDetalleComanda = await _httpClient.GetAsync($"{_baseUrlComandas}/{comandaId}/detalles");
        if (respDetalleComanda.IsSuccessStatusCode)
        {
            string apiResponse = await respDetalleComanda.Content.ReadAsStringAsync();
            vm.Detalles = JsonConvert.DeserializeObject<List<DetalleComandaViewModel>>(apiResponse);
        }

        // 5. Obtener métodos de pago
        HttpResponseMessage respMetodos = await _httpClient.GetAsync("https://localhost:7296/api/MetodosPago");
        if (respMetodos.IsSuccessStatusCode)
        {
            string apiResponse = await respMetodos.Content.ReadAsStringAsync();
            vm.MetodosPago = JsonConvert.DeserializeObject<List<MetodoPago>>(apiResponse);
        }


        return View(vm);
    }


    // POST: Factura/GenerarFactura
    [HttpPost]
    public async Task<IActionResult> GenerarFactura(FacturaViewModel model)
    {
        // 1️⃣ Guardar comprobante
        var comprobanteData = new
        {
            comanda_id = model.Comprobante.ComandaId,
            empleado_id = model.Comprobante.EmpleadoId,
            tipo_comprobante_id = model.Comprobante.TipoComprobanteId,
            cliente_id = model.Comprobante.ClienteId,
            sub_total = model.Comprobante.SubTotal,
            igv_total = model.Comprobante.IgvTotal,
            precio_total_pedido = model.Comprobante.PrecioTotalPedido,
            fecha_emision=model.Comprobante.FechaEmision
   
        };

        var jsonComprobante = JsonConvert.SerializeObject(comprobanteData);
        var contentComprobante = new StringContent(jsonComprobante, Encoding.UTF8, "application/json");
        var respComp = await _httpClient.PostAsync(_baseUrlComprobante, contentComprobante);

        Console.WriteLine($"EmpleadoId: {model.Comprobante.EmpleadoId}, ClienteId: {model.Comprobante.ClienteId}, SubTotal: {model.Comprobante.SubTotal}");


        if (!respComp.IsSuccessStatusCode)
            return View("Nuevo", model);

        var comprobanteResponse = await respComp.Content.ReadAsStringAsync();

        // Deserializar como objeto dinámico
        var respObj = JsonConvert.DeserializeObject<dynamic>(comprobanteResponse);

        // Obtener el comprobante real desde 'data'
        int comprobanteId = respObj.data.id;

        // Opcional: crear un objeto Comprobante para usar en la vista
        var comprobanteGuardado = new Comprobante
        {
            Id = comprobanteId,
            SubTotal = respObj.data.sub_total,
            IgvTotal = respObj.data.igv_total,
            PrecioTotalPedido = respObj.data.precio_total_pedido
        };


        // 2️⃣ Enviar pagos uno por uno usando el endpoint DetalleComprobante
        foreach (var pago in model.DetalleComprobante)
        {
            var pagoData = new
            {
                comprobante_id = comprobanteId,
                metodo_pago_id = pago.metodo_pago_id,
                monto_pago = pago.monto_pago
            };

            // Convertir a JSON
            var jsonPago = JsonConvert.SerializeObject(pagoData);

            // 🔹 Imprimir en consola (debug)
            Console.WriteLine("Enviando pago: " + jsonPago);

            var contentPago = new StringContent(jsonPago, Encoding.UTF8, "application/json");

            // Llamada a la API
            var respPago = await _httpClient.PostAsync("https://localhost:7296/api/DetalleComprobante", contentPago);

            if (!respPago.IsSuccessStatusCode)
            {
                // Leer mensaje de error de la respuesta (si viene)
                var errorMsg = await respPago.Content.ReadAsStringAsync();

                // Guardar mensaje en TempData para mostrar en la vista
                TempData["ErrorPago"] = $"Error al registrar pago (método {pago.metodo_pago_id}): {errorMsg}";

                // 🔹 También imprimir en consola
                Console.WriteLine($"Error al enviar pago: {errorMsg}");
            }
        }

        // 3️⃣ Liberar la mesa
        // Si model.Comanda es null, puedes traerla
        if (model.Comanda == null)
        {
            var respComanda = await _httpClient.GetAsync($"{_baseUrlComandas}/{model.Comprobante.ComandaId}");
            if (respComanda.IsSuccessStatusCode)
            {
                string apiResponse = await respComanda.Content.ReadAsStringAsync();
                model.Comanda = JsonConvert.DeserializeObject<Comanda>(apiResponse);
            }
        }

        // Ahora sí puedes liberar la mesa
        if (model.Comanda?.MesaId > 0)
        {
            var urlLiberar = $"https://localhost:7296/api/Mesas/{model.Comanda.MesaId}/liberar";
            var liberarResponse = await _httpClient.PutAsync(urlLiberar, null);
            if (!liberarResponse.IsSuccessStatusCode)
                Console.WriteLine("Error al liberar la mesa: " + liberarResponse.StatusCode);
        }

        // 4️⃣ Actualizar estado de la comanda a 3
        if (model.Comprobante.ComandaId > 0)
        {
            var urlActualizarEstado = $"https://localhost:7296/api/Comandas/{model.Comprobante.ComandaId}/estado";

            // Convertir el nuevo estado a JSON
            var estadoJson = new StringContent("3", Encoding.UTF8, "application/json");

            var respEstado = await _httpClient.PutAsync(urlActualizarEstado, estadoJson);

            if (!respEstado.IsSuccessStatusCode)
            {
                Console.WriteLine("Error al actualizar estado de la comanda: " + respEstado.StatusCode);
                var mensajeError = await respEstado.Content.ReadAsStringAsync();
                Console.WriteLine(mensajeError);
            }
        }

        // 3️⃣ Guardar mensaje de confirmación
        TempData["FacturaGenerada"] = comprobanteGuardado.Id;

        // 4️⃣ Redirigir a la misma vista para mostrar modal
        return RedirectToAction("Nuevo", new { comandaId = model.Comprobante.ComandaId });
    }



}
