using FutFut.Profile.Service.Entities;

namespace FutFut.Profile.Service.Mapping;

public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<ProfileEntity, ProfileDto>();
        CreateMap<ProfileDto, ProfileEntity>();
        CreateMap<CreateProfileDto, ProfileEntity>();
    }
}