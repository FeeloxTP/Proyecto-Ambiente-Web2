using Proyecto.Application.Services.Implementations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.DTOs;

public record PaisDTO
{
    [Required(ErrorMessage = "{0} es requerido")]
    [Display(Name = "Código")]
    [StringLength(2, ErrorMessage = "{0} debe tener como máximo {1} caracteres.")]
    public string IdPais { get; set; } = null!;

    [Display(Name = "Nombre País")]
    [Required(ErrorMessage = "{0} es requerido")]
    public string Descripcion { get; set; } = null!;
}
