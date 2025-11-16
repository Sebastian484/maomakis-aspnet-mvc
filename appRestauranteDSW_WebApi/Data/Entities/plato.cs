using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Data.Entities;

public partial class plato
{
    [Key]
    [StringLength(255)]
    [Unicode(false)]
    public string id { get; set; } = Guid.NewGuid().ToString();

    [StringLength(255)]
    [Unicode(false)]
    public string? imagen { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? nombre { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? precio_plato { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? categoria_plato_id { get; set; }

    [ForeignKey("categoria_plato_id")]
    [InverseProperty("plato")]
    public virtual categoria_plato? categoria_plato { get; set; }

    [InverseProperty("plato")]
    public virtual ICollection<detalle_comanda> detalle_comanda { get; set; } = new List<detalle_comanda>();
}
