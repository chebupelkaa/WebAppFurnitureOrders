﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.BLL.Profiles
{
    public class ProviderProfile:Profile
    {
        public ProviderProfile()
        {
            CreateMap<ProviderDTO, Provider>().ReverseMap();
        }
    }
}
