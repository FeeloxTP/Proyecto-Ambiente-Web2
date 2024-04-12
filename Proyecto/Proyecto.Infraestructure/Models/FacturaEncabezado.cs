﻿using System;
using System.Collections.Generic;

namespace Proyecto.Infraestructure.Models;

public partial class FacturaEncabezado
{
    public int IdFactura { get; set; }

    public int IdTarjeta { get; set; }

    public Guid IdCliente { get; set; }

    public DateTime FechaFacturacion { get; set; }

    public int EstadoFactura { get; set; } = 1;

    public string TajetaNumero { get; set; } = null!;

    public virtual ICollection<FacturaDetalle> FacturaDetalle { get; set; } = new List<FacturaDetalle>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Tarjeta IdTarjetaNavigation { get; set; } = null!;
}
