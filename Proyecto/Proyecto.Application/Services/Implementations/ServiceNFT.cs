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

public class ServiceNFT : IServiceNFT
{
    private readonly IRepositoryNFT _repository;
    private readonly IMapper _mapper;

    public ServiceNFT(IRepositoryNFT repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ICollection<NFTDTO>> FindByNameAsync(string name)
    {
        var list = await _repository.FindByNameAsync(name);
        var collection = _mapper.Map<ICollection<NFTDTO>>(list);
        return collection;
    }

    public async Task<ICollection<NFTDTO>> FindByNameXReporteAsync(string name)
    {
        var list = await _repository.FindByNameAsync(name);
        var collection = _mapper.Map<ICollection<NFTDTO>>(list);
        return collection;
    }


    public async Task<Guid> AddAsync(NFTDTO dto)
    {
        var objectMapped = _mapper.Map<ActivoNft>(dto);
        return await _repository.AddAsync(objectMapped);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<ICollection<NFTDTO>> FindByDescriptionAsync(string description)
    {
        var list = await _repository.FindByDescriptionAsync(description);
        var collection = _mapper.Map<ICollection<NFTDTO>>(list);
        return collection;
    }

    public async Task<NFTDTO> FindByIdAsync(Guid id)
    {
        var @object = await _repository.FindByIdAsync(id);
        var objectMapped = _mapper.Map<NFTDTO>(@object);
        return objectMapped;
    }

    public async Task<ICollection<NFTDTO>> ListAsync()
    {
        // Get data from Repository
        var list = await _repository.ListAsync();
        // Map List<Entity> to ICollection<XXXXXXDTO>
        var collection = _mapper.Map<ICollection<NFTDTO>>(list);
        // Return Data
        return collection;
    }

    public async Task UpdateAsync(Guid id, NFTDTO dto)
    {
        var objectMapped = _mapper.Map<ActivoNft>(dto);
        await _repository.UpdateAsync(id, objectMapped);
    }
}
