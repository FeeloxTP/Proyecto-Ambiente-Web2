using AutoMapper;
using Proyecto.Application.DTOs;
using Proyecto.Application.Services.Interfaces;
using Proyecto.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.Services.Implementations
{
    public class ServiceMovimientosCompras : IServiceMovimientosCompras
    {
        private readonly IRepositoryMovimientosCompras _repositoryMovimientosCompras;
        private readonly IMapper _mapper;

        public ServiceMovimientosCompras(IRepositoryMovimientosCompras repositoryMovimientosCompras, IMapper mapper)
        {
            _repositoryMovimientosCompras = repositoryMovimientosCompras;
            _mapper = mapper;
        }

        public async Task<MovimientosComprasDTO> FindByIdAsync(int id)
        {
            var @object = await _repositoryMovimientosCompras.FindByIdAsync(id);
            var objectMapped = _mapper.Map<MovimientosComprasDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<MovimientosComprasDTO>> FindByIdNFT(Guid id)
        {
            var list = await _repositoryMovimientosCompras.FindByIdNFT(id);
            var collection = _mapper.Map<ICollection<MovimientosComprasDTO>>(list);
            return collection;
        }

        public async Task<ICollection<MovimientosComprasDTO>> ListAsync()
        {
            var list = await _repositoryMovimientosCompras.ListAsync();
            var collection = _mapper.Map<ICollection<MovimientosComprasDTO>>(list);
            return collection;
        }
    }
}
