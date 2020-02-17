using AutoMapper;
using CRUD3.Models.Account;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD3.Profiles
{
    public class LogInProfile: Profile
    {
        public LogInProfile()
        {
            CreateMap<RegisterViewModel, LogInViewModel>().ReverseMap();
            CreateMap<UserInfo, LogInViewModel>().ReverseMap();
            CreateMap<UserInfo, RegisterViewModel>().ReverseMap();
        }
    }
}
