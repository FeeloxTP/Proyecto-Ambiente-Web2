using System;
using System.Collections.Generic;

namespace Proyecto.Infraestructure.Models;

public partial class Perfil
{
    public int IdPerfil { get; set; }

    public string DescripcionPerfil { get; set; } = null!;

    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}
