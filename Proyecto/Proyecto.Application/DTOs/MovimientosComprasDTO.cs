using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.DTOs
{
    public record  MovimientosComprasDTO
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public Guid IdNft { get; set; }

        public Guid ClienteId { get; set; }

        public bool Estado { get; set; }
    }
}
