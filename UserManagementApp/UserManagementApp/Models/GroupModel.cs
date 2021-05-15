using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UserManagementApp.Models
{
    public class GroupModel
    {
        public int GroupID { get; set; }

        [Required]
        public string GroupName { get; set; }

        [Required]
        public string GroupDescription { get; set; }

        public bool GroupActive { get; set; }

        public List<MenuModel> MenuList { get; set; }
    }

    public class MenuModel
    {
        public int MenuID { get; set; }

        public string MenuName { get; set; }

        public bool IsSelected { get; set; }
    }
}