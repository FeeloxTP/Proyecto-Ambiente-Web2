using Proyecto.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPerfil
    {
        Task<ICollection<Perfil>> FindByDescriptionAsync(string description);
        Task<ICollection<Perfil>> ListAsync();
        Task<Perfil> FindByIdAsync(int id);
        Task<int> AddAsync(Perfil entity);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, Perfil entity);
    }
}
