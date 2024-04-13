using Microsoft.EntityFrameworkCore;
using Proyecto.Infraestructure.Data;
using Proyecto.Infraestructure.Models;
using Proyecto.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Infraestructure.Repository.Implementations
{
    public class RepositoryMovimientosCompras : IRepositoryMovimientosCompras
    {
        private readonly ProyectoContext _context;

        public RepositoryMovimientosCompras(ProyectoContext context)
        {
            _context = context;
        }

        public async Task<MovimientosCompras> FindByIdAsync(int id)
        {
            var @object = await _context.Set<MovimientosCompras>().FindAsync(id);
            return @object!;
        }

        public async Task<ICollection<MovimientosCompras>> FindByIdNFT(Guid id)
        {
            var collection = await _context
                                    .Set<MovimientosCompras>()
                                    .Where(p => p.IdNft == id)
                                    .ToListAsync();
            return collection;
        }

        public async Task<ICollection<MovimientosCompras>> ListAsync()
        {
            var collection = await _context.Set<MovimientosCompras>().AsNoTracking().ToListAsync();
            return collection;
        }
    }
}
