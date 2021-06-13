using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SqlSugarAndEntity.DataTransferObject.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarAndEntity.AutoMapperConfig
{
    public static class AutoMapperHelper
    {
        public static void AddAutoMapper(this IServiceCollection service)
        {
            service.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserDTO, userinfo>().ReverseMap();
        }
    }
}
