using System;
using System.Collections.Generic;

namespace Proyecto.Infraestructure.Models;

public partial class ActivoNft
{
    public Guid IdNft { get; set; }

    public string Nombre { get; set; } = null!;

    public string Autor { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Inventario { get; set; }

    public byte[] Imagen { get; set; } = null!;

    public virtual ICollection<FacturaDetalle> FacturaDetalle { get; set; } = new List<FacturaDetalle>();
}
