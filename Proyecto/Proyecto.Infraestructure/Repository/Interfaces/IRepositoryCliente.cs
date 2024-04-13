using Proyecto.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryCliente
    {
        Task<ICollection<Cliente>> FindByNameAsync(string name);
        Task<ICollection<Cliente>> ListAsync();
        Task<Cliente> FindByIdAsync(Guid id);
        Task<Guid> AddAsync(Cliente entity);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(Guid id, Cliente entity);
    }
}
