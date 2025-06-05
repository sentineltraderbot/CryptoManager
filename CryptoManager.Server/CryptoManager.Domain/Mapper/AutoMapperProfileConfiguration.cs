using AutoMapper;
using CryptoManager.Domain.DTOs;
using CryptoManager.Domain.Entities;
using CryptoManager.Domain.IntegrationEntities.Facebook;
using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoManager.Domain.Mapper
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
            : this("MyProfile")
        {
        }

        protected AutoMapperProfileConfiguration(string profileName)
        : base(profileName)
        {
            CreateMap<Exchange, ExchangeDTO>();
            CreateMap<ExchangeDTO, Exchange>();
            CreateMap<Asset, AssetDTO>();
            CreateMap<AssetDTO, Asset>();
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDTO, Order>();
            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<TokenSaleDTO, TokenSale>();
            CreateMap<OrderItemDTO, OrderItem>();
            CreateMap<FacebookUserData, ApplicationUser>()
                .ForMember(dest => dest.FacebookId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.Email))
                .ForMember(dest => dest.PictureUrl, opts => opts.MapFrom(src => src.Picture.Data.Url))
                .ForMember(dest => dest.Id, opts => opts.Ignore());
            CreateMap<GoogleJsonWebSignature.Payload, ApplicationUser>()
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.Email))
                .ForMember(dest => dest.GoogleId, opts => opts.MapFrom(src => src.Subject))
                .ForMember(dest => dest.EmailConfirmed, opts => opts.MapFrom(src => src.EmailVerified))
                .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.GivenName))
                .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.FamilyName))
                .ForMember(dest => dest.Locale, opts => opts.MapFrom(src => src.Locale))
                .ForMember(dest => dest.PictureUrl, opts => opts.MapFrom(src => src.Picture));
            CreateMap<ApplicationUser, ApplicationUserDTO>()
                .ForMember(dest => dest.ImageURL, opts => opts.MapFrom(src => src.PictureUrl));;
        }
    }
}
