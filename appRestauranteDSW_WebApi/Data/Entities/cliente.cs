using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Data.Entities;

public partial class cliente
{
    [Key]
    public int id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? apellido { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? dni { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? nombre { get; set; }

    [InverseProperty("cliente")]
    public virtual ICollection<comprobante> comprobante { get; set; } = new List<comprobante>();
}
