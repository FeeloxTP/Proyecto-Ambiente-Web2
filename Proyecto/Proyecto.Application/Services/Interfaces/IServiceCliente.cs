using Proyecto.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.Services.Interfaces;

public interface IServiceCliente
{
    Task<ICollection<ClienteDTO>> FindByNameAsync(string name);
    Task<ICollection<ClienteDTO>> ListAsync();
    Task<ClienteDTO> FindByIdAsync(Guid id);
    Task<Guid> AddAsync(ClienteDTO dto);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Guid id, ClienteDTO dto);
}
