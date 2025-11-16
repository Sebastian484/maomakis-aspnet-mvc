using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Data.Entities;

public partial class mesa
{
    [Key]
    public int id { get; set; }

    public int? cantidad_asientos { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? estado { get; set; }

    [InverseProperty("mesa")]
    public virtual ICollection<comanda> comanda { get; set; } = new List<comanda>();
}
