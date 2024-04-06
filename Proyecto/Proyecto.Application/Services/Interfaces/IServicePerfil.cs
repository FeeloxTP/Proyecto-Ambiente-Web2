using Proyecto.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.Services.Interfaces
{
    public interface IServicePerfil
    {
        Task<ICollection<PerfilDTO>> FindByDescriptionAsync(string DescripcionPefil);
        Task<ICollection<PerfilDTO>> ListAsync();
        Task<PerfilDTO> FindByIdAsync(int id);
        Task<int> AddAsync(PerfilDTO dto);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, PerfilDTO dto);
    }
}
