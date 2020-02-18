using AutoMapper;
using CRUD3.Models.Account;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD3.Profiles
{
    public class AdministrationProfile : Profile
    {
        public AdministrationProfile()
        {
            CreateMap<RoleInfo, RoleViewModel>().ReverseMap();
            CreateMap<UserInfo, UserViewModel>().ReverseMap();
            CreateMap<UserInfo, ApplicationUser>().ReverseMap();
            CreateMap<RoleInfo, ApplicationRole>().ReverseMap();
        }
    }
}
