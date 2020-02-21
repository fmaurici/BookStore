using AutoMapper;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Profiles
{
    public class AdministrationProfile : Profile
    {
        public AdministrationProfile()
        {
            CreateMap<UserInfo, ApplicationUser>().ReverseMap();
            CreateMap<RoleInfo, ApplicationRole>().ReverseMap();
        }
    }
}
