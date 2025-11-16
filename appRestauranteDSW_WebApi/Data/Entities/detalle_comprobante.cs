using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Data.Entities;

public partial class detalle_comprobante
{
    [Key]
    public int id { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? monto_pago { get; set; }

    public int? comprobante_id { get; set; }

    public int? metodo_pago_id { get; set; }

    [ForeignKey("comprobante_id")]
    [InverseProperty("detalle_comprobante")]
    public virtual comprobante? comprobante { get; set; }

    [ForeignKey("metodo_pago_id")]
    [InverseProperty("detalle_comprobante")]
    public virtual metodo_pago? metodo_pago { get; set; }
}
