using API_HomeWork.Models;
using AutoMapper;
using Repositories.ModelForCreate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Profiles
{
    public class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<Hotel,HotelWithOutEnyThing>();
        }
    }
}
