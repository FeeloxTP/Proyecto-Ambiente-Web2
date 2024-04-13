using Proyecto.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.Services.Interfaces
{
    public interface IServiceMovimientosCompras
    {
        Task<ICollection<MovimientosComprasDTO>> ListAsync();
        Task<MovimientosComprasDTO> FindByIdAsync(int id);
        Task<ICollection<MovimientosComprasDTO>> FindByIdNFT(Guid id);

    }
}
