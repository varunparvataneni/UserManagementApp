using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementApp.Models;
namespace UserManagementApp.Service
{
    public interface IUserManagement
    {
        UserModel IsValidUser(string LoginName, string Password);
        UserModel SubmitUser(UserModel userModel);
        GroupModel SubmitGroup(GroupModel groupModel);
        IEnumerable<UserModel> GetAllUsers();
        IEnumerable<GroupModel> GetAllGroups();
        UserModel GetUserDetails(int UserID);
        GroupModel GetGroupDetails(int GroupID);
        IEnumerable<MenuModel> GetMenuItems();
        void LinkGroups(int GroupID,List<MenuModel> menuModels,string Mode);
    }
}
