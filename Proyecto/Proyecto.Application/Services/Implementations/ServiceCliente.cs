using AutoMapper;
using Proyecto.Application.DTOs;
using Proyecto.Application.Services.Interfaces;
using Proyecto.Infraestructure.Models;
using Proyecto.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.Services.Implementations
{
    public class ServiceCliente : IServiceCliente
    {
        private readonly IRepositoryCliente _repository;
        private readonly IMapper _mapper;

        public ServiceCliente(IRepositoryCliente repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Guid> AddAsync(ClienteDTO dto)
        {
            var objectMapped = _mapper.Map<Cliente>(dto);
            return await _repository.AddAsync(objectMapped);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<ICollection<ClienteDTO>> FindByNameAsync(string description)
        {
            var list = await _repository.FindByNameAsync(description);
            var collection = _mapper.Map<ICollection<ClienteDTO>>(list);
            return collection;
        }

        public async Task<ClienteDTO> FindByIdAsync(Guid id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<ClienteDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<ClienteDTO>> ListAsync()
        {
            // Get data from Repository
            var list = await _repository.ListAsync();
            // Map List<Entity> to ICollection<XXXXXXDTO>
            var collection = _mapper.Map<ICollection<ClienteDTO>>(list);
            // Return Data
            return collection;
        }

        public async Task UpdateAsync(Guid id, ClienteDTO dto)
        {
            var objectMapped = _mapper.Map<Cliente>(dto);
            await _repository.UpdateAsync(id, objectMapped);
        }
    }
}
