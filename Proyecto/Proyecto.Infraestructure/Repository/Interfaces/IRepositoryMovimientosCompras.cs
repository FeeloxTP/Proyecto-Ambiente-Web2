using Proyecto.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryMovimientosCompras
    {
        Task<ICollection<MovimientosCompras>> ListAsync();
        Task<MovimientosCompras> FindByIdAsync(int id);
        Task<ICollection<MovimientosCompras>> FindByIdNFT(Guid id);

    }
}
