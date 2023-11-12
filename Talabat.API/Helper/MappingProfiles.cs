using AutoMapper;
using Talabat.API.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.API.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductReturnDto>()
                .ForMember(productDto => productDto.ProductBrand, O => O.MapFrom(product => product.ProductBrand.Name))
                .ForMember(productDto => productDto.ProductType, O => O.MapFrom(product => product.ProductType.Name))
                .ForMember(productDto => productDto.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Talabat.Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<AddressDto, Talabat.Core.Entities.Order_Aggregation.Address>()
                .ForMember(address => address.Fname, O => O.MapFrom(addressDto => addressDto.FirstName))
                .ForMember(address => address.Lname, O => O.MapFrom(addressDto => addressDto.LastName));
                
        }
    }
}
