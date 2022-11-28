using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NASATechAPI.Entities;
using NASATechAPI.Interfaces;
using NASATechAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace NASATechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {

        private readonly IRepositoryAsync<User> _repository;
        private readonly IMapper _mapper;
        const string HASH_STRING = "Pa$$word!00";
        public LoginController(IRepositoryAsync<User> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<UserModel> Login(LoginModel User)
        {
            //User.Password = Decrypt(User.Password);
            UserModel UserToLogin = new UserModel();
            try
            {
                var data = await _repository.Find(c => c.Email.Equals(User.Email));
                UserToLogin = _mapper.Map<UserModel>(data);

                if (User.Password.Equals(DecodeFrom64(UserToLogin.Password)))
                {
                    return UserToLogin;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }

        private static string Decrypt(string Encrypted)
        {
            byte[] data = Convert.FromBase64String(Encrypted);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripDES = new TripleDESCryptoServiceProvider();

            tripDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(HASH_STRING));
            tripDES.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripDES.CreateDecryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return UTF8Encoding.UTF8.GetString(result);
        }

        //this function Convert to Decord your Password
        private string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
    }
}
