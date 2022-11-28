using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NASATechAPI.Entities;
using NASATechAPI.Interfaces;
using NASATechAPI.Models;
using System.Text;
using System.Security.Cryptography;
using System.Linq;

namespace NASATechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IRepositoryAsync<User> _repository;
        private readonly IRepositoryAsync<UserRoles> _roles;
        private readonly IMapper _mapper;
        public UsersController(IRepositoryAsync<User> repository,
            IRepositoryAsync<UserRoles> roles,
            IMapper mapper)
        {
            _repository = repository;
            _roles = roles;
            _mapper = mapper;
        }

        // GET: Users
        [HttpGet]
        public async Task<List<UserModel>> Index()
        {
            var data = await _repository.GetAll();
            var roles = await _roles.GetAll();
            var userRoles = _mapper.Map<List<UserRoleModel>>(roles);
            var users = _mapper.Map<List<UserModel>>(data);


            return users;
        }

        // Post: Users
        [HttpPost]
        public async Task<UserModel> Save(UserModel NewUser)
        {
            NewUser.Password = EncodePasswordToBase64(NewUser.Password);
            User user = _mapper.Map<User>(NewUser);
            try
            {
                 await _repository.Insert(user);
            }
            catch (Exception ex)
            {
                return null;
            }
            return NewUser;
        }

        //this function Convert to Encord your Password
        private static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
    }
}
