using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebAPI.Database;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private ApplicationDatabase _db;
        private IConfiguration _config;
        public SignInController(ApplicationDatabase db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }
   
        [HttpPost]
        public object Post([FromBody] UserCredencialModel Body)
        {
            string Hash = Register.ComputeSha256Hah(Body.UserName, Body.Password, _config["SHA256:Salt"]);
            int UserID = _db.User.Where(u => u.UserName == Body.UserName && u.Password == Hash).Select(u => u.UserID).FirstOrDefault();
            if (UserID != 0 )
            {
                return new { token = GenerateJSONWebToken(Body, UserID) };
            }
            else
            {
                return new { message = "not successful" };
            }
        }
        private string GenerateJSONWebToken(UserCredencialModel UserCred, int UserID)
        {
            SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            Claim[] Claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sid, UserID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                Claims,
                expires: DateTime.Now.AddYears(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        
      
    }

    
}