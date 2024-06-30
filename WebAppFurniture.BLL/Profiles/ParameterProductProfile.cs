using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.BLL.DTO;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.BLL.Profiles
{
    public class ParameterProductProfile:Profile
    {
        public ParameterProductProfile()
        {
            CreateMap<ParameterProductDTO, ParameterProduct>().ReverseMap();
        }
    }
}
