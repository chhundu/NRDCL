using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Models.Acc
{
    public class ChangePasswordModel
    {
        [Required, DataType(DataType.Password),Display(Name ="Current Password")]
        public string CurrentPassword { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "New Password")]
        public string  NewPassword { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage ="Your passwords doesn't match")]
        public string  ConfirmNewPassword { get; set; }
    }
}
