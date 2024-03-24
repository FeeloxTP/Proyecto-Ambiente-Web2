using System;
using System.Collections.Generic;

namespace Proyecto.Infraestructure.Models;

public partial class Pais
{
    public string IdPais { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Cliente> Cliente { get; set; } = new List<Cliente>();
}
