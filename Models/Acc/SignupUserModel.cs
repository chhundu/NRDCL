using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Models.Acc
{
    public class SignupUserModel
    {
        [Required(ErrorMessage = "Please enter your First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage ="Please enter your email")]
        [Display(Name ="Email address")]
        [EmailAddress(ErrorMessage ="Please enter a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Please enter a strong password")]
        [Compare("ConfirmPassword", ErrorMessage ="Password does not match")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string  ConfirmPassword { get; set; }

        [NotMapped,Required]
        public string  RoleName { get; set; }
    }
}
