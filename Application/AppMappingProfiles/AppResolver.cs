using Application.AppDtos;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.AppMappingProfiles
{
    
    public class AppResolver : IValueResolver<Product, ProductAppDto, string>
    {
        private readonly IConfiguration _config;

        public AppResolver(IConfiguration config)
        {
            _config = config;
        }
        public string Resolve(Product source, ProductAppDto destination, string destMember, ResolutionContext context)
        {

            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"] + source.PictureUrl;
            }

            return null;
        }
    }
}
