using AutoMapper;
using BusinessLogicLayer.DTO;
using DataAccessLayer.entities;

namespace BusinessLogicLayer.Mappers;

public class ProductUpdateRequestToProductMappingProfile : Profile
{
    public ProductUpdateRequestToProductMappingProfile()
    {
        CreateMap<ProductUpdateRequest, Product>();
    }
}