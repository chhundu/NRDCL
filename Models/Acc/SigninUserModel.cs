using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Models.Acc
{
    public class SigninUserModel
    {
        [Required,EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string  Password { get; set; }
        [Display(Name ="Remember me")]
        public bool Rememberme{ get; set; }

    }
}
