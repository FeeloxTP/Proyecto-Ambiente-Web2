using System;
using System.Collections.Generic;

namespace Proyecto.Infraestructure.Models;

public partial class CriptoWallet
{
    public int IdWallet { get; set; }

    public Guid IdCliente { get; set; }

    public int Disponibles { get; set; }

    public int Canjeados { get; set; }

    public int Recibidos { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;
}
