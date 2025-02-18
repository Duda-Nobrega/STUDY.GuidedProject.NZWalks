using AutoMapper;

namespace NZWalksAPI.mappings
{
    public class AutoMappersProfiles : Profile
    {
        public AutoMappersProfiles()
        {
            CreateMap<UserDto, UserDomain>().ReverseMap();
        }
    }

    public class UserDto
    {
        public string FullName { get; set; }
    }

    public class UserDomain
    {
        public string FullName { get; set; }
    }
}
