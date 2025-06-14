using AutoMapper;
using FutFut.Notify.Contracts;
using FutFut.Notify.Service.Data.Entities;

namespace FutFut.Notify.Service.Mapping;

public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterDeviceDto, DeviceEntity>();
        
        CreateMap<NotificationDto, NotificationEntity>();
        CreateMap<NotificationEntity, NotificationDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
    }
}