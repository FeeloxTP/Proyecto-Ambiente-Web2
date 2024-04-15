using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Proyecto.Application.DTOs;

public class FacturaDTO
{
    [Display(Name = "No Factura")]
    [ValidateNever]
    public int IdFactura { get; set; }
    [Display(Name = "Tipo Tarjeta")]

    [Required(ErrorMessage = "{0} es requerido")]
    public int IdTarjeta { get; set; }
    [Display(Name = "Cliente")]

    [Required(ErrorMessage = "{0} es requerido")]
    public Guid IdCliente { get; set; }

    [Required(ErrorMessage = "{0} es requerido")]
    [Display(Name = "No Tarjeta")]
    public string TajetaNumero { get; set; } = null!;

    public bool EstadoFactura { get; set; }

    public decimal Total { get; set; }

    public List<FacturaDetalleDTO> ListFacturaDetalle = null!;
}
