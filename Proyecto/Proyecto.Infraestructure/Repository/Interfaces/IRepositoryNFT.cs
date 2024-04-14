using Proyecto.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Infraestructure.Repository.Interfaces;

public interface IRepositoryNFT
{
    Task<ICollection<ActivoNft>> FindByNameAsync(string name);
    Task<ICollection<ActivoNft>> FindByNameXReporteAsync(string description);
    Task<ICollection<ActivoNft>> FindByDescriptionAsync(string name);
    Task<ICollection<ActivoNft>> ListAsync();
    Task<ActivoNft> FindByIdAsync(Guid id);
    Task<Guid> AddAsync(ActivoNft entity);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Guid id, ActivoNft entity);
}
