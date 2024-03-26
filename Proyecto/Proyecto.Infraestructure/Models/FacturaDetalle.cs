using System;
using System.Collections.Generic;

namespace Proyecto.Infraestructure.Models;

public partial class FacturaDetalle
{
    public int IdFactura { get; set; }

    public int Secuencia { get; set; }

    public Guid IdNft { get; set; }

    public int Cantidad { get; set; }

    public decimal Precio { get; set; }

    public virtual FacturaEncabezado IdFacturaNavigation { get; set; } = null!;

    public virtual ActivoNft IdNftNavigation { get; set; } = null!;
}
