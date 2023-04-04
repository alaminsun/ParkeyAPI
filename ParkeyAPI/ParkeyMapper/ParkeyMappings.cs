﻿using AutoMapper;
using ParkeyAPI.Models;
using ParkeyAPI.Models.Dtos;

namespace ParkeyAPI.ParkeyMapper
{
    public class ParkeyMappings : Profile
    {
        public ParkeyMappings()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
        }
    }
}
