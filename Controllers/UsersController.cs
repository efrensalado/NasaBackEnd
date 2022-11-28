using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NASATechAPI.Entities;
using NASATechAPI.Interfaces;
using NASATechAPI.Models;
using System.Text;
using System.Security.Cryptography;

namespace NASATechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IRepositoryAsync<User> _repository;
        private readonly IMapper _mapper;
        const string HASH_STRING = "Pa$$word!00";
        public UsersController(IRepositoryAsync<User> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: Users
        [HttpGet]
        public async Task<List<UserModel>> Index()
        {
            var data = await _repository.GetAll();
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

        private static string Encrypt(string Decrypted)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(Decrypted);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripDES = new TripleDESCryptoServiceProvider();

            tripDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(HASH_STRING));
            tripDES.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripDES.CreateEncryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return Convert.ToBase64String(result);
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
