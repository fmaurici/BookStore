using CRUD3.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace CRUD3.Profiles
{
    public class BookOperationProfile : Profile
    {
        public BookOperationProfile()
        {
            CreateMap<BookOperation, BookOperationViewModel>().ReverseMap();
        }
        
    }
}
