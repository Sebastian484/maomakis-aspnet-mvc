using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Data.Entities;

public partial class estado_comanda
{
    [Key]
    public int id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? estado { get; set; }

    [InverseProperty("estado_comanda")]
    public virtual ICollection<comanda> comanda { get; set; } = new List<comanda>();
}
