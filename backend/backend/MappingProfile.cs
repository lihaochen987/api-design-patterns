using AutoMapper;
using backend.Contracts;
using backend.Models;

namespace backend;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UpdateProductRequest, Product>();
    }
}