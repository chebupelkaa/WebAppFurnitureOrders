using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.DAL.Entities;
using AutoMapper;

namespace WebAppFurniture.BLL.Profiles
{
    public class ParameterProfile:Profile
    {
        public ParameterProfile()
        {
            CreateMap<ParameterDTO, Parameter>().ReverseMap();
        }
    }
}
