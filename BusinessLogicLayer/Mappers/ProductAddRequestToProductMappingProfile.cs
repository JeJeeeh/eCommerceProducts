using AutoMapper;
using BusinessLogicLayer.DTO;
using DataAccessLayer.entities;

namespace BusinessLogicLayer.Mappers;

public class ProductAddRequestToProductMappingProfile : Profile
{
    public ProductAddRequestToProductMappingProfile()
    {
        CreateMap<ProductAddRequest, Product>()
            .ForMember(dest => dest.ProductID,
                opt => opt.Ignore());
    }
}