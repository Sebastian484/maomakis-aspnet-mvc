using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace appRestauranteDSW_WebApi.Data.Entities;

public partial class usuario
{
    [Key]
    public int id { get; set; }

    public int? codigo { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? contrasena { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? correo { get; set; }

    public bool? verificado { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? token_verificacion { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? fecha_token { get; set; }

    [InverseProperty("usuario")]
    public virtual ICollection<empleado> empleado { get; set; } = new List<empleado>();
}
