using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Xjp2Backend.Controllers
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "{0} is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(128, MinimumLength = 6, ErrorMessage = "minimum Length of {0} is 6.")]
        public string Password { get; set; }
    }

    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "{0} is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string RefreshToken { get; set; }
    }
}
