using AutoMapper;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubicX_223020new.BusinessLogic.AutoMapperProfile
{
    public class BusinessLogicProfile : Profile
    {
        public BusinessLogicProfile()//конструктор
        {
            CreateMap<UserRto, UserUpdateBlo>()
                .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                .ForMember(x => x.Birthday, x => x.MapFrom(m => m.Birthday))
                .ForMember(x => x.LastName, x => x.MapFrom(m => m.LastName))
                .ForMember(x => x.FirstName, x => x.MapFrom(m => m.FirstName))
                .ForMember(x => x.Patronymic, x => x.MapFrom(m => m.Patronymic))
                .ForMember(x => x.Password, x => x.MapFrom(m => m.Password))
                .ForMember(x => x.Email, x => x.MapFrom(m => m.Email))
                .ForMember(x => x.Login, x => x.MapFrom(m => m.Login))
                .ForMember(x => x.PhoneNumderPrefix, x => x.MapFrom(m => m.PhoneNumberPrefix))
                .ForMember(x => x.PhoneNumder, x => x.MapFrom(m => m.PhoneNumber))
                .ForMember(x => x.IsBoy, x => x.MapFrom(m => m.IsBoy))
                .ForMember(x => x.AvatarUrl, x => x.MapFrom(m => m.AvatarUrl));

            CreateMap<UserRto, UserInformationBlo>()
                    .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                    .ForMember(x => x.Birthday, x => x.MapFrom(m => m.Birthday))
                    .ForMember(x => x.LastName, x => x.MapFrom(m => m.LastName))
                    .ForMember(x => x.FirstName, x => x.MapFrom(m => m.FirstName))
                    .ForMember(x => x.Patronymic, x => x.MapFrom(m => m.Patronymic))
                    .ForMember(x => x.Email, x => x.MapFrom(m => m.Email))
                    .ForMember(x => x.PhoneNumderPrefix, x => x.MapFrom(m => m.PhoneNumberPrefix))
                    .ForMember(x => x.PhoneNumder, x => x.MapFrom(m => m.PhoneNumber))
                    .ForMember(x => x.IsBoy, x => x.MapFrom(m => m.IsBoy))
                    .ForMember(x => x.AvatarUrl, x => x.MapFrom(m => m.AvatarUrl));
        }
    }
}
