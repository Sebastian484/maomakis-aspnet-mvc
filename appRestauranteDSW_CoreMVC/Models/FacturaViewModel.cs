namespace appRestauranteDSW_CoreMVC.Models
{
    public class FacturaViewModel
    {
        public Comprobante Comprobante { get; set; }
        public Comanda Comanda { get; set; }
        public List<DetalleComandaViewModel> Detalles { get; set; } = new();
        public List<TipoComprobante> TiposComprobante { get; set; } = new();

        // Métodos de pago disponibles
        public List<MetodoPago> MetodosPago { get; set; } = new();

        public List<DetalleComprobante> DetalleComprobante { get; set; } = new();
    }
}
