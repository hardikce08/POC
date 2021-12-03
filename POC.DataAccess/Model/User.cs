using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.DataAccess.Model
{
    public class User
    {
        public int UserID { get; set; }
        [Required(ErrorMessage = "UserName is Required")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ContactNumber { get; set; }

        //public string ImageName { get; set; }

        //public int CompanyId { get; set; }
        //public string ReturnUrl { get; set; }
        public bool RememberSignIn { get; set; }
        public string UserGuid { get; set; }
    }
    public class UserProfileEditModel
    {
        public int UserID { get; set; }

        public string EmailAddress { get; set; }



        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ContactNumber { get; set; }



        [Required(ErrorMessage = "Old Password is Required")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "New Password is Required")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Re-Type Password is Required")]
        [Compare("NewPassword", ErrorMessage = "The New Password and Re-Type Password fields do not match.")]
        public string ReTypePassword { get; set; }
    }
}
