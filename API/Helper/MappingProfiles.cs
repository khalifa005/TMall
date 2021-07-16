using API.Dtos;
using AutoMapper;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helper
{
    public class MappingProfiles : Profile
    {
        //keys o is options d is destination s is source
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d=> d.ProductBrand, o=> o.MapFrom(s=> s.ProductBrand.Name))
                .ForMember(d=> d.ProductType, o=> o.MapFrom(s=> s.ProductType.Name))
                .ForMember(d=> d.PictureUrl, o=> o.MapFrom<ProductUrlResolver>());//to put a full path for the image url
        }
    }
}
