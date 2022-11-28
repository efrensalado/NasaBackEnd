using AutoMapper;
using NASATechAPI.Entities;
using NASATechAPI.Models;

namespace NASATechAPI.mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}
