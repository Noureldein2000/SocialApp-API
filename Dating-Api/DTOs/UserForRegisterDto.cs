using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dating_Api.DTOs
{
    public class UserForRegisterDto
    {
        public UserForRegisterDto()
        {
            CreationDate = DateTime.Now;
            LastActive = DateTime.Now;
        }

        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(10,MinimumLength =4,ErrorMessage ="You must specify password between 4 and 10 charchater.")]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string KnowsAs { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastActive { get; set; }
    }
}
