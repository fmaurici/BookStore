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
            CreateMap<UserInfo, ApplicationUser>().ReverseMap();
            CreateMap<RoleInfo, ApplicationRole>().ReverseMap();

            CreateMap<RoleInfo, RoleViewModel>().ReverseMap();

            CreateMap<UserInfo, UserViewModel>()
                .ForMember(
                 vm => vm.SelectedRoles, opt => opt.MapFrom(
                     src => src.Roles.Select(r => r.Name)
                     )
                );

            CreateMap<UserViewModel, UserInfo>()
                .ForMember(
                 entity => entity.Roles, opt => opt.MapFrom(
                     src => src.SelectedRoles.Select(selectedRole => new RoleInfo() { Name = selectedRole })
                     )
                );
        }
    }
}
