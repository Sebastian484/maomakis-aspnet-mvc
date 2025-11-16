using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Data.Entities;

public partial class caja
{
    [Key]
    [StringLength(255)]
    [Unicode(false)]
    public string id { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? establecimiento_id { get; set; }

    [InverseProperty("caja")]
    public virtual ICollection<comprobante> comprobante { get; set; } = new List<comprobante>();

    [ForeignKey("establecimiento_id")]
    [InverseProperty("caja")]
    public virtual establecimiento? establecimiento { get; set; }
}
