using AutoMapper;
using BusinessLogicLayer.DTO;
using DataAccessLayer.entities;

namespace BusinessLogicLayer.Mappers;

public class ProductToProductResponseMappingProfile : Profile
{
    public ProductToProductResponseMappingProfile()
    {
        CreateMap<Product, ProductResponse>();
    }
}