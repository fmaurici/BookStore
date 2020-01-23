using AutoMapper;
using CRUD3.Models;
using Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD3.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            //Acá le estaría diciendo a Automapper que puede convertir mi Client en un ClientViewModel y viceversa
            CreateMap<Client, ClientViewModel>().ReverseMap();
        }
    }
}
