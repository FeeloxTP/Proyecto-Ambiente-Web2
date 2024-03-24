using System;
using System.Collections.Generic;

namespace Proyecto.Infraestructure.Models;

public partial class Usuario
{
    public string Login { get; set; } = null!;

    public int IdPerfil { get; set; }

    public string Password { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public bool? Estado { get; set; }

    public virtual Perfil IdPerfilNavigation { get; set; } = null!;
}
