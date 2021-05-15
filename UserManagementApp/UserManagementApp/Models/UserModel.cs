using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace UserManagementApp.Models
{
    public class UserModel
    {
        public int UserID { get; set; }

        [Required]
        public string LoginName { get; set; }

        [Required]
        public string UserDescription { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "GroupName")]
        public int Group_ID { get; set; }

        public IEnumerable<MenuModel> MenuList { get; set; }

    }
}