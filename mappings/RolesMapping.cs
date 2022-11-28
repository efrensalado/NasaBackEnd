using AutoMapper;
using NASATechAPI.Entities;
using NASATechAPI.Models;

namespace NASATechAPI.mappings
{
    public class RolesMapping : Profile
    {
        public RolesMapping()
        {
            CreateMap<UserRoles, UserRoleModel>().ReverseMap();
        }
    }
}
