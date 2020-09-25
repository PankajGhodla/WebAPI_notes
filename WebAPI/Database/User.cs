using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Database
{
    public class User
    {
        [Required]
        [Key]
        public int UserID { get; set; }

        [Required]
        public string UserName {get; set;}
        
        [Required] 
        public string Password { get; set; }

        public ICollection<UserNote> UserNote { get; set; }
    }
}