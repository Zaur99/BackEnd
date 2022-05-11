using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Models.ViewModels.Identity
{
    public class RegisterModel
    {
        [Required(ErrorMessage ="FirstName boş buraxıla bilməz")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName boş buraxıla bilməz")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "UserName boş buraxıla bilməz")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password boş buraxıla bilməz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "FirstName boş buraxıla bilməz")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
