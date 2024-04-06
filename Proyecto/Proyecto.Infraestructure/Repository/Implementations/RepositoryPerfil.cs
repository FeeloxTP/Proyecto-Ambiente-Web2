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
    public class RepositoryPerfil : IRepositoryPerfil
    {
        private readonly ProyectoContext _context;

        public RepositoryPerfil(ProyectoContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Perfil entity)
        {
            await _context.Set<Perfil>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdPerfil;
        }

        public async Task DeleteAsync(int id)
        {
            var @object = await FindByIdAsync(id);
            _context.Remove(@object);
            _context.SaveChanges();
        }

        public async Task<ICollection<Perfil>> FindByDescriptionAsync(string description)
        {
            var collection = await _context
                                     .Set<Perfil>()
                                     .Where(p => p.DescripcionPerfil.Contains(description))
                                     .ToListAsync();
            return collection;
        }

        public async Task<Perfil> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Perfil>().FindAsync(id);
            return @object!;
        }

        public async Task<ICollection<Perfil>> ListAsync()
        {
            var collection = await _context.Set<Perfil>().AsNoTracking().ToListAsync();
            return collection;
        }

        public async Task UpdateAsync(int id, Perfil entity)
        {
            await _context.SaveChangesAsync();
        }
    }
}
