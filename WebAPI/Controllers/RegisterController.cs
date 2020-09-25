using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Database;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Register : ControllerBase
    {
        private ApplicationDatabase _db;
        private IConfiguration _config;
        public Register(ApplicationDatabase db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        //Post
        [HttpPost]
        public object Post([FromBody] UserCredencialModel Body)
        {
            if (ModelState.IsValid && _db.User.Where(u => u.UserName == Body.UserName).ToList().Count == 0)
            {
                List<UserNote> InitialNote = new List<UserNote> { new UserNote { Heading = "<h1>Heading</h1>", Note = "<p> </p>" } };

                _db.User.Add(new User { 
                    UserName = Body.UserName, 
                    Password = ComputeSha256Hah(Body.UserName, Body.Password, _config["SHA256:Salt"]), 
                    UserNote= InitialNote 
                });
                _db.SaveChanges();

                return new { message = "successful" };
            }
            else
            {
                return new { message = "not successful" };
            }
        }

        public static string ComputeSha256Hah(string UserName, string Password, string Salt)
        {
            using (SHA256 Hash = SHA256.Create())
            {
                string RawData = string.Format("{2}{0}:{1}{2}", UserName, Password, Salt);
                byte[] bytes = Hash.ComputeHash(Encoding.UTF8.GetBytes(RawData));

                //Converts the bytes array into a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}