using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.DTOs;

public record FacturaDetalleDTO
{
    public int IdFactura { get; set; }
    [Display(Name = "No")]
    public int Secuencia { get; set; }
    [Display(Name = "Código")]
    public string DescripcionProducto { get; set; } = default!;

    public Guid IdNFT { get; set; }
    [Display(Name = "Cantidad")]
    public int Cantidad { get; set; }
    [DisplayFormat(DataFormatString = "{0:n2}")]
    [Display(Name = "Precio")]
    public decimal Precio { get; set; }
    //[DisplayFormat(DataFormatString = "{0:n2}")]
    //[Display(Name = "Impuesto")]
    //public decimal Impuesto { get; set; }
    //[DisplayFormat(DataFormatString = "{0:n2}")]
    //[Display(Name = "Total")]
    public decimal TotalLinea { get; set; } 

}
