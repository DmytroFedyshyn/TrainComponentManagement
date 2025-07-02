using AutoMapper;
using TrainComponentManagement.BLL.DTOs;
using TrainComponentManagement.DAL.Models;

namespace TrainComponentManagement.BLL.Mapping
{
    public class ComponentProfile : Profile
    {
        public ComponentProfile()
        {
            CreateMap<Component, ComponentDto>();

            CreateMap<CreateOrUpdateComponentDto, Component>();

            CreateMap<ComponentDto, Component>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
