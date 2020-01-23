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
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            //Acá le decimos a automapper que puede transformar un Book en un BookViewModel con el CreateMap y viceversa con el ReverseMap
            //Como tenemos un elemento especial en nuestro viewModel (lista de SelectListItems), le pido a automaper que lo mapee de una forma particular
            //Para ver un ejemplo mas facil, ir al Client Profile
            CreateMap<Book, BookViewModel>()
                .ForMember(
                 vm => vm.Clients, opt => opt.MapFrom(
                     src => src.BookClients.Select(bc =>
                         new SelectListItem() { Value = bc.Client.Id.ToString(), Text = bc.Client.Name }
                         )
                     )
                )
                .ReverseMap();

            //CreateMap<Book, BookViewModel>().ReverseMap(); //Asi sería el mapping simple, si no tuvieramos propiedades que difieran entre VM y Entidad
        }
    }
}
