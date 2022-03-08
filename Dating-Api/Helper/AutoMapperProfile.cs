using AutoMapper;
using Dating_Api.DTOs;
using Dating_Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dating_Api.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                }).ForMember(dest => dest.Age, opt =>
                {
                    opt.MapFrom(d => d.DateOfBirth.CalculateAge());
                });

            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
            {
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
            }).ForMember(dest => dest.Age, opt =>
            {
                opt.MapFrom(d => d.DateOfBirth.CalculateAge());
            });

            CreateMap<Photo, PhotoForDetailedDto>();
            CreateMap<User, UserForUpdateDto>().ReverseMap();

            CreateMap<Photo, PhotoFromReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();

            CreateMap<UserForRegisterDto, User>();

            CreateMap<MessageForCreationDto, Message>().ReverseMap();

            CreateMap<Message, MessageToReturnDto>()
                 .ForMember(dest => dest.SenderPhotoUrl, opt =>
                 {
                     opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(p => p.IsMain).Url);
                 }).ForMember(dest => dest.RecipientPhotoUrl, opt =>
                 {
                     opt.MapFrom(d => d.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url);
                 });

        }
    }
}
