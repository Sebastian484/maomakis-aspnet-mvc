using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Data.Entities;

public partial class comanda
{
    [Key]
    public int id { get; set; }

    public int? cantidad_asientos { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? fecha_emision { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? precio_total { get; set; }

    public int? empleado_id { get; set; }

    public int? estado_comanda_id { get; set; }

    public int? mesa_id { get; set; }

    [InverseProperty("comanda")]
    public virtual ICollection<comprobante> comprobante { get; set; } = new List<comprobante>();

    [InverseProperty("comanda")]
    public virtual ICollection<detalle_comanda> detalle_comanda { get; set; } = new List<detalle_comanda>();

    [ForeignKey("empleado_id")]
    [InverseProperty("comanda")]
    public virtual empleado? empleado { get; set; }

    [ForeignKey("estado_comanda_id")]
    [InverseProperty("comanda")]
    public virtual estado_comanda? estado_comanda { get; set; }

    [ForeignKey("mesa_id")]
    [InverseProperty("comanda")]
    public virtual mesa? mesa { get; set; }
}
