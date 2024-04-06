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
    public class ServicePerfil : IServicePerfil
    {
        private readonly IRepositoryPerfil _repository;
        private readonly IMapper _mapper;

        public ServicePerfil(IRepositoryPerfil repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(PerfilDTO dto)
        {
            var objectMapped = _mapper.Map<Perfil>(dto);
            return await _repository.AddAsync(objectMapped);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<ICollection<PerfilDTO>> FindByDescriptionAsync(string DescripcionTarjeta)
        {
            var list = await _repository.FindByDescriptionAsync(DescripcionTarjeta);
            var collection = _mapper.Map<ICollection<PerfilDTO>>(list);
            return collection;
        }

        public async Task<PerfilDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<PerfilDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<PerfilDTO>> ListAsync()
        {
            // Get data from Repository
            var list = await _repository.ListAsync();
            // Map List<Bodega> to ICollection<BodegaDTO>
            var collection = _mapper.Map<ICollection<PerfilDTO>>(list);
            // Return Data
            return collection;
        }

        public async Task UpdateAsync(int id, PerfilDTO dto)
        {
            var objectMapped = _mapper.Map<Perfil>(dto);
            await _repository.UpdateAsync(id, objectMapped);
        }
    }
}
