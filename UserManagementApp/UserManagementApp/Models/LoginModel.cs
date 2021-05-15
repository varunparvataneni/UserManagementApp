using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UserManagementApp.Models
{
    public class LoginModel
    {

        [Required(ErrorMessage = "Please Enter LoginName")]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}