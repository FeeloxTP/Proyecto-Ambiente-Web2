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
        public string Login { get; set; } = default!;
        public int IdPerfil { get; set; }
        public string DescripcionPerfil { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Nombre { get; set; } = default!;
        public string Apellidos { get; set; } = default!;
        public bool Estado { get; set; } 
    }
}
