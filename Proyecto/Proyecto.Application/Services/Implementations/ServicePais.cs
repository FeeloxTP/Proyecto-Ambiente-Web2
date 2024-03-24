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

namespace Proyecto.Application.Services.Implementations;

public class ServicePais : IServicesPais
{
    private readonly IRepositoryPais _repository;
    private readonly IMapper _mapper;

    public ServicePais(IRepositoryPais repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<string> AddAsync(PaisDTO dto)
    {
        // Map BodegaDTO to Bodega
        var objectMapped = _mapper.Map<Pais>(dto);

        // Return
        return await _repository.AddAsync(objectMapped);
    }

    public async Task DeleteAsync(string id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<ICollection<PaisDTO>> FindByDescriptionAsync(string description)
    {
        var list = await _repository.FindByDescriptionAsync(description);
        var collection = _mapper.Map<ICollection<PaisDTO>>(list);
        return collection;

    }

    public async Task<PaisDTO> FindByIdAsync(string id)
    {
        var @object = await _repository.FindByIdAsync(id);
        var objectMapped = _mapper.Map<PaisDTO>(@object);
        return objectMapped;
    }

    public async Task<ICollection<PaisDTO>> ListAsync()
    {
        // Get data from Repository
        var list = await _repository.ListAsync();
        // Map List<Bodega> to ICollection<BodegaDTO>
        var collection = _mapper.Map<ICollection<PaisDTO>>(list);
        // Return Data
        return collection;
    }

    public async Task UpdateAsync(string id, PaisDTO dto)
    {
        var objectMapped = _mapper.Map<Pais>(dto);
        await _repository.UpdateAsync(id, objectMapped);
    }

    
}
