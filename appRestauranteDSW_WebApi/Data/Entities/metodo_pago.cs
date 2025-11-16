using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Data.Entities;

public partial class metodo_pago
{
    [Key]
    public int id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? metodo { get; set; }

    [InverseProperty("metodo_pago")]
    public virtual ICollection<detalle_comprobante> detalle_comprobante { get; set; } = new List<detalle_comprobante>();
}
