using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Data.Entities;

public partial class comprobante
{
    [Key]
    public int id { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? descuento_total { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? fecha_emision { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? igv_total { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? precio_total_pedido { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? sub_total { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? caja_id { get; set; }

    public int? cliente_id { get; set; }

    public int? comanda_id { get; set; }

    public int? empleado_id { get; set; }

    public int? tipo_comprobante_id { get; set; }

    [ForeignKey("caja_id")]
    [InverseProperty("comprobante")]
    public virtual caja? caja { get; set; }

    [ForeignKey("cliente_id")]
    [InverseProperty("comprobante")]
    public virtual cliente? cliente { get; set; }

    [ForeignKey("comanda_id")]
    [InverseProperty("comprobante")]
    public virtual comanda? comanda { get; set; }

    [InverseProperty("comprobante")]
    public virtual ICollection<detalle_comprobante> detalle_comprobante { get; set; } = new List<detalle_comprobante>();

    [ForeignKey("empleado_id")]
    [InverseProperty("comprobante")]
    public virtual empleado? empleado { get; set; }

    [ForeignKey("tipo_comprobante_id")]
    [InverseProperty("comprobante")]
    public virtual tipo_comprobante? tipo_comprobante { get; set; }
}
