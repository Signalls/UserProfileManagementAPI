using AutoMapper;
using UserProfileData.Domain;
using UserProfileData.DTO;

namespace UserManagementSystemAPI.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserProfile, UserProfileUpdateDto>().ReverseMap();
        }
    }
}
