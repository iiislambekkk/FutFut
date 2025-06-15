using FutFut.Profile.Service.Dtos;
using FutFut.Profile.Service.Entities;

namespace FutFut.Profile.Service.Mapping;

public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<ProfileEntity, ProfileDto>();
        CreateMap<ProfileDto, ProfileEntity>();
        CreateMap<CreateProfileDto, ProfileEntity>();

        CreateMap<UpdateProfileDto, ProfileEntity>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<FriendShipRequestDto, FriendShipRequestEntity>();

        CreateMap<PlayedHistoryEntity, PlayedHistoryDto>();
        CreateMap<PlayedHistoryDto, PlayedHistoryEntity>();

        CreateMap<AboutPhotoEntity, AboutPhotoDto>();
        CreateMap<AboutPhotoDto, AboutPhotoEntity>();
    }
}