using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Database
{
    public class UserNote
    {
        [Required]
        [Key]
        public int NoteID { get; set; }

        [Required]
        public string Heading { get; set; }

        [Required]
        public string Note { get; set; }
    }
}
