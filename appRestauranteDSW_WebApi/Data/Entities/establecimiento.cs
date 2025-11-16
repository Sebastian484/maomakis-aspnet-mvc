using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Data.Entities;

public partial class establecimiento
{
    [Key]
    [StringLength(255)]
    [Unicode(false)]
    public string id { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? direccion { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? nombre { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ruc { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? telefono { get; set; }

    [InverseProperty("establecimiento")]
    public virtual ICollection<caja> caja { get; set; } = new List<caja>();
}
