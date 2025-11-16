using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace appRestauranteDSW_CoreMVC.Models
{
    public class DetalleComprobante
    {
        public int id { get; set; }

        public decimal? monto_pago { get; set; }

        public int? comprobante_id { get; set; }

        public int? metodo_pago_id { get; set; }

        // Relación con otras tablas (opcional)
        public virtual Comprobante Comprobante { get; set; }
        public virtual MetodoPago MetodoPago { get; set; }
    }
}
