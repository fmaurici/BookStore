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
