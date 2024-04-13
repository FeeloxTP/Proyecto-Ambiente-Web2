using System;
using System.Collections.Generic;

namespace Proyecto.Infraestructure.Models;

public partial class MovimientosCompras
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public Guid IdNft { get; set; }

    public Guid ClienteId { get; set; }

    public bool Estado { get; set; }
}
