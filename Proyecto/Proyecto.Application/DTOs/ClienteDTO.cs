using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.DTOs;

public class ClienteDTO
{
    [Display(Name = "Id")]
    public Guid IdCliente { get; set; }

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "{0} es requerido")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "{0} debe ser entre {1} y {2} caracteres")]
    public string Nombre { get; set; } = null!;

    [Display(Name = "Primer apellido")]
    [Required(ErrorMessage = "{0} es requerido")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "{0} debe ser entre {1} y {2} caracteres")]
    public string Apellido1 { get; set; } = null!;

    [Display(Name = "Segundo apellido")]
    [Required(ErrorMessage = "{0} es requerido")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "{0} debe ser entre {1} y {2} caracteres")]
    public string Apellido2 { get; set; } = null!;

    [Display(Name = "Correo electrónico")]
    [Required(AllowEmptyStrings = true, ErrorMessage = "Ingrese un {0} Válido")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Display(Name = "País")]
    [Required(ErrorMessage = "{0} es requerido")]
    public string IdPais { get; set; } = null!;
}
