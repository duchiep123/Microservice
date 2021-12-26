using AutoMapper;
using CarsService.API.Models;
using GarageManagementModels;
using GarageManagementModels.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            //CreateMap<Car, ReturnGetCarModel>();
            CreateMap<Garage, GarageReadDto>().ForMember(dest => dest.Cars, opts => opts.MapFrom(p=> p.Cars));
        }
    }
}
