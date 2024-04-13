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
    public class RepositoryCliente : IRepositoryCliente
    {
        private readonly ProyectoContext _context;

        public RepositoryCliente(ProyectoContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(Cliente entity)
        {
            await _context.Set<Cliente>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdCliente;
        }

        public async Task DeleteAsync(Guid id)
        {
            var @object = await FindByIdAsync(id);
            _context.Remove(@object);
            _context.SaveChanges();
        }

        public async Task<Cliente> FindByIdAsync(Guid id)
        {
            var @object = await _context.Set<Cliente>().FindAsync(id);
            return @object!;
        }

        public async Task<ICollection<Cliente>> FindByNameAsync(string description)
        {
            description = description.Replace(' ', '%');
            description = "%" + description + "%";
            FormattableString sql = $@"select * from Cliente where Nombre+Apellido1+Apellido2 like  {description}  ";

            var collection = await _context.Cliente.FromSql(sql).AsNoTracking().ToListAsync();
            return collection;
        }

        public async Task<ICollection<Cliente>> ListAsync()
        {
            var collection = await _context.Set<Cliente>().AsNoTracking().ToListAsync();
            return collection;
        }

        public async Task UpdateAsync(Guid id, Cliente entity)
        {
            var @object = await FindByIdAsync(id);
            @object.Nombre = entity.Nombre;
            @object.Apellido1 = entity.Apellido1;
            @object.Apellido2 = entity.Apellido2;
            @object.Email = entity.Email;
            @object.IdPais = entity.IdPais;
            await _context.SaveChangesAsync();
        }
    }
}
