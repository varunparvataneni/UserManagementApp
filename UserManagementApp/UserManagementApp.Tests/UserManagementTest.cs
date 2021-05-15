using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserManagementApp.Service;
using UserManagementApp.Service.Implementation;

namespace UserManagementApp.Tests
{
    [TestClass]
    public class UserManagementTest
    {

        private readonly IUserManagement _userManagement;

        public UserManagementTest()
        {
            this._userManagement = new UserManagement();
        }

        [TestMethod]
        public void IsValidUserTest()
        {
            var result = _userManagement.IsValidUser("Admin", "admin");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUserDetailsTest()
        {
            var result = _userManagement.GetUserDetails(1);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetGroupDetailsTest()
        {
            var result = _userManagement.GetGroupDetails(1);
            Assert.IsNotNull(result);
        }
    }
}
