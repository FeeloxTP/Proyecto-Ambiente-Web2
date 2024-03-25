using Proyecto.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.Services.Interfaces;

public interface IServicesPais
{
    Task<ICollection<PaisDTO>> FindByDescriptionAsync(string description);
    Task<ICollection<PaisDTO>> ListAsync();
    Task<PaisDTO> FindByIdAsync(string id);
    Task<string> AddAsync(PaisDTO dto);
    Task DeleteAsync(string id);
    Task UpdateAsync(string id, PaisDTO dto);

}
