using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.DTOs
{
    public record PerfilDTO
    {
        [Required(ErrorMessage = "{0} es requerido")]
        [Display(Name = "Código")]
        public int IdPerfil { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "{0} es requerido")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{0} debe ser entre {1} y {2} caracteres")]
        public string DescripcionPerfil { get; set; } = null!;
    }
}
