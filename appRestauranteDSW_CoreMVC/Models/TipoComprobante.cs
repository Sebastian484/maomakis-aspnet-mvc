using Newtonsoft.Json;

namespace appRestauranteDSW_CoreMVC.Models
{
    public class TipoComprobante
    {
        public int Id { get; set; }

        [JsonProperty("tipo")]
        public string? Tipo { get; set; }
    }
}
