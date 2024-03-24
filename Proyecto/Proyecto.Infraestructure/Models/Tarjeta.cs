using System;
using System.Collections.Generic;

namespace Proyecto.Infraestructure.Models;

public partial class Tarjeta
{
    public int IdTarjeta { get; set; }

    public string DescripcionTarjeta { get; set; } = null!;

    public bool Estado { get; set; }

    public virtual ICollection<FacturaEncabezado> FacturaEncabezado { get; set; } = new List<FacturaEncabezado>();
}
