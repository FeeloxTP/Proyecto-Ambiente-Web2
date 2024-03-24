using System;
using System.Collections.Generic;

namespace Proyecto.Infraestructure.Models;

public partial class Cliente
{
    public string IdCliente { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido1 { get; set; } = null!;

    public string Apellido2 { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string IdPais { get; set; } = null!;

    public virtual ICollection<CriptoWallet> CriptoWallet { get; set; } = new List<CriptoWallet>();

    public virtual ICollection<FacturaEncabezado> FacturaEncabezado { get; set; } = new List<FacturaEncabezado>();

    public virtual Pais IdPaisNavigation { get; set; } = null!;
}
