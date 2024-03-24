using Proyecto.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Infraestructure.Repository.Interfaces;

public interface IRepositoryPais
{
    Task<ICollection<Pais>> FindByDescriptionAsync(string description);
    Task<ICollection<Pais>> ListAsync();
    Task<Pais> FindByIdAsync(string id);
    Task<string> AddAsync(Pais entity);
    Task DeleteAsync(string id);
    Task UpdateAsync(string id, Pais entity);

}
