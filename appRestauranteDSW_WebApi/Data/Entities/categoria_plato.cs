using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Data.Entities;

public partial class categoria_plato
{
    [Key]
    [StringLength(255)]
    [Unicode(false)]
    public string id { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? nombre { get; set; }

    [InverseProperty("categoria_plato")]
    public virtual ICollection<plato> plato { get; set; } = new List<plato>();
}
