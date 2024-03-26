using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.DTOs;

public record TarjetaDTO
{
    [Required(ErrorMessage = "{0} es requerido")]
    [Display(Name = "Código")]

    public int IdTarjeta { get; set; }

    [Display(Name = "Descripción")]
    [Required(ErrorMessage = "{0} es requerido")]
    [StringLength(20, MinimumLength = 4, ErrorMessage = "{0} debe ser entre {1} y {2} caracteres")]
    public string DescripcionTarjeta { get; set; } = null!;

    [Required(ErrorMessage = "{0} es requerido")]
    [Display(Name = "Estado")]
    public bool Estado { get; set; }
}

