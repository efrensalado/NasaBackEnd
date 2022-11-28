using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NASATechAPI.Entities;
using NASATechAPI.Interfaces;
using NASATechAPI.Models;

namespace NASATechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : Controller
    {
        private readonly IRepositoryAsync<UserRoles> _repository;
        private readonly IMapper _mapper;
        public RolesController(IRepositoryAsync<UserRoles> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: Users
        [HttpGet]
        public async Task<List<UserRoleModel>> Index()
        {
            var data = await _repository.GetAll();
            var roles = _mapper.Map<List<UserRoleModel>>(data);
            return roles;
        }
    }
}
