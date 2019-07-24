using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.WebUI.Models
{
    public class LoginViewModel
    {
        [Required]
        [UIHint("username")]
        public string Username { get; set; }

        [Required]
        [UIHint("username")]
        public string Password { get; set; }
    }
}
