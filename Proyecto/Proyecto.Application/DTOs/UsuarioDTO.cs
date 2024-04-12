using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.DTOs
{
    public record UsuarioDTO
    {
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Solamente números y letras")]
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "{0} es requerido")]
        public string Login { get; set; } = default!;

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "{0} es requerido")]
        public int IdPerfil { get; set; }

        [Display(Name = "Descripción del rol")]
        [Required(ErrorMessage = "{0} es requerido")]
        public string DescripcionPerfil { get; set; } = default!;

        [StringLength(15, MinimumLength = 6, ErrorMessage = "Error en política de largo de contraseña")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Solamente números y letras")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "{0} es requerido")]
        public string Password { get; set; } = default!;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "{0} es requerido")]
        public string Nombre { get; set; } = default!;

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "{0} es requerido")]
        public string Apellidos { get; set; } = default!;

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "{0} es requerido")]
        public bool Estado { get; set; } 
    }
}
