using System.ComponentModel.DataAnnotations;

namespace WebAPI.Database
{
    public class SignInModel
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}