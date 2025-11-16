using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Data.Entities;

public partial class empleado
{
    [Key]
    public int id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? apellido { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? dni { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? fecha_registro { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? nombre { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? telefono { get; set; }

    public int? cargo_id { get; set; }

    public int? usuario_id { get; set; }

    [ForeignKey("cargo_id")]
    [InverseProperty("empleado")]
    public virtual cargo? cargo { get; set; }

    [InverseProperty("empleado")]
    public virtual ICollection<comanda> comanda { get; set; } = new List<comanda>();

    [InverseProperty("empleado")]
    public virtual ICollection<comprobante> comprobante { get; set; } = new List<comprobante>();

    [ForeignKey("usuario_id")]
    [InverseProperty("empleado")]
    public virtual usuario? usuario { get; set; }
}
