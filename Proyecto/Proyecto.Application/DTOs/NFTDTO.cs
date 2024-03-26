using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.DTOs;

public class NFTDTO
{
    [Display(Name = "Código")]
    [Required(ErrorMessage = "{0} es requerido")]
    public Guid IdNft { get; set; }

    [Required(ErrorMessage = "{0} es requerido")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "{0} es requerido")]
    public string Autor { get; set; } = default!;

    [Display(Name = "Precio")]
    [Range(0, 999999999, ErrorMessage = "El valor mínimo es {0}")]
    [Required(ErrorMessage = "{0} es requerido")]
    public decimal Precio { get; set; }

    [Display(Name = "Inventario")]
    [Required(ErrorMessage = "{0} es requerido")]
    public int Inventario { get; set; }

    [Display(Name = "Imagen")]
    [Required(ErrorMessage = "{0} es requerido")]
    public byte[] Imagen { get; set; } = null!;
}
