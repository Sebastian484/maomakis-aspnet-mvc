using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Data.Entities;

public partial class tipo_comprobante
{
    [Key]
    public int id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? tipo { get; set; }

    [InverseProperty("tipo_comprobante")]
    public virtual ICollection<comprobante> comprobante { get; set; } = new List<comprobante>();
}
