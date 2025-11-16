using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Data.Entities;

public partial class detalle_comanda
{
    [Key]
    public int id { get; set; }

    public int? cantidad_pedido { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? observacion { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? precio_unitario { get; set; }

    public int? comanda_id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? plato_id { get; set; }

    [ForeignKey("comanda_id")]
    [InverseProperty("detalle_comanda")]
    public virtual comanda? comanda { get; set; }

    [ForeignKey("plato_id")]
    [InverseProperty("detalle_comanda")]
    public virtual plato? plato { get; set; }
}
