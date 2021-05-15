using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BC = BCrypt.Net.BCrypt;
using UserManagementApp.Models;

namespace UserManagementApp.Service.Implementation
{
    public class UserManagement : IUserManagement
    {
        private readonly string _connectionString;
        public UserManagement()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["UserManagementConnectionstring"].ConnectionString;
        }

        public UserModel IsValidUser(string LoginName,string Password)
        {
            try
            {
                return GetAllUsers().Where(x => x.LoginName == LoginName && BC.Verify(Password, x.Password)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserModel SubmitUser(UserModel userModel)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("SubmitUser", sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 1200;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter("@UserID", userModel.UserID));
                        sqlCmd.Parameters.Add(new SqlParameter("@LoginName", userModel.LoginName));
                        sqlCmd.Parameters.Add(new SqlParameter("@UserDescription", userModel.UserDescription));
                        sqlCmd.Parameters.Add(new SqlParameter("@EmailAddress",userModel.EmailAddress));
                        if (userModel.UserID == 0)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter("@Password", BC.HashPassword(userModel.Password)));
                        }
                        sqlCmd.Parameters.Add(new SqlParameter("@Group_ID", userModel.Group_ID));

                        sqlConnection.Open();
                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                userModel.UserID = Convert.ToInt32(reader["UserID"]);
                            }
                        }
                    }
                }
                return userModel;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public GroupModel SubmitGroup(GroupModel groupModel)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand("SubmitGroup", sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 1200;
                        sqlCmd.CommandType = CommandType.StoredProcedure;


                        SqlParameter[] Parameters = {
                        new SqlParameter("@GroupID",groupModel.GroupID),
                        new SqlParameter("@GroupName",groupModel.GroupName),
                        new SqlParameter("@GroupDescription",groupModel.GroupDescription),
                        new SqlParameter("@GroupActive",Convert.ToBoolean(groupModel.GroupActive))
                        };

                        sqlCmd.Parameters.AddRange(Parameters);
                        sqlConnection.Open();
                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                groupModel.GroupID = Convert.ToInt32(reader["GroupID"]);
                            }
                        }
                    }
                }
                return groupModel;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            try
            {
                List<UserModel> users = new List<UserModel>();
                using (SqlConnection SqlConnection = new SqlConnection(_connectionString))
                {
                    SqlConnection.Open();
                    using (SqlCommand cmd = new SqlCommand("select * from Users", SqlConnection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new UserModel()
                                {
                                    UserID = Convert.ToInt32(reader["UserID"]),
                                    LoginName = Convert.ToString(reader["LoginName"]),
                                    UserDescription= Convert.ToString(reader["UserDescription"]),
                                    Password = Convert.ToString(reader["Password"]),
                                    ConfirmPassword = Convert.ToString(reader["Password"]),
                                    EmailAddress = Convert.ToString(reader["EmailAddress"]),
                                    Group_ID = (reader["Group_ID"] != DBNull.Value) ? Convert.ToInt32(reader["Group_ID"]) : Convert.ToInt32(null)
                                    
                                });
                            }
                        }
                    }
                }
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<GroupModel> GetAllGroups()
        {
            try
            {
                List<GroupModel> Groups = new List<GroupModel>();
                using (SqlConnection SqlConnection = new SqlConnection(_connectionString))
                {
                    SqlConnection.Open();
                    using (SqlCommand cmd = new SqlCommand("select * from Groups", SqlConnection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Groups.Add(new GroupModel()
                                {
                                    GroupID = Convert.ToInt32(reader["GroupID"]),
                                    GroupName = Convert.ToString(reader["GroupName"]),
                                    GroupDescription = Convert.ToString(reader["GroupDescription"]),
                                    GroupActive = Convert.ToBoolean(reader["GroupActive"])
                                });
                            }
                        }
                    }
                }
                return Groups;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserModel GetUserDetails(int UserID)
        {
            try
            {
               UserModel userdetails= GetAllUsers().Where(x => x.UserID == UserID).FirstOrDefault();
                userdetails.MenuList = GetGroupLinkedMenuItems(userdetails.Group_ID);

                return userdetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GroupModel GetGroupDetails(int GroupID)
        {
            try
            {
                GroupModel group = GetAllGroups().Where(x => x.GroupID == GroupID).FirstOrDefault();
                group.MenuList = GetGroupLinkedMenuItems(GroupID).ToList();
                return group;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<MenuModel> GetMenuItems()
        {
            try
            {
                List<MenuModel> Menuitems = new List<MenuModel>();
                using (SqlConnection SqlConnection = new SqlConnection(_connectionString))
                {
                    SqlConnection.Open();
                    using (SqlCommand cmd = new SqlCommand("select * from MenuItems", SqlConnection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Menuitems.Add(new MenuModel()
                                {
                                    MenuID = Convert.ToInt32(reader["MenuID"]),
                                    MenuName = Convert.ToString(reader["MenuName"])

                                });
                            }
                        }
                    }
                }
                return Menuitems;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<MenuModel> GetGroupLinkedMenuItems(int GroupID)
        {
            try
            {
                List<MenuModel> Menuitems = new List<MenuModel>();
                using (SqlConnection SqlConnection = new SqlConnection(_connectionString))
                {
                    SqlConnection.Open();
                    using (SqlCommand cmd = new SqlCommand("GetGroupMenuOptions", SqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@GroupID", GroupID));
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Menuitems.Add(new MenuModel()
                                {
                                    MenuID = Convert.ToInt32(reader["MenuID"]),
                                    MenuName = Convert.ToString(reader["MenuName"]),
                                    IsSelected = Convert.ToBoolean(reader["IsSelected"])

                                });
                            }
                        }
                    }
                }
                return Menuitems;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LinkGroups(int GroupID,List<MenuModel> menuModels, string Mode)
        {
            try
            {
                if (Mode == "Create")
                {
                    using (SqlConnection SqlConnection = new SqlConnection(_connectionString))
                    {
                        SqlConnection.Open();
                        foreach (var menu in menuModels)
                        {
                            using (SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[LinkGroup]([MenuID],[GroupID],[IsSelected])VALUES(@MenuID,@GroupID,@IsSelected)", SqlConnection))
                            {
                                cmd.Parameters.AddWithValue("@MenuID", menu.MenuID);
                                cmd.Parameters.AddWithValue("@GroupID", GroupID);
                                cmd.Parameters.AddWithValue("@IsSelected", menu.IsSelected);

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                else
                {
                    using (SqlConnection SqlConnection = new SqlConnection(_connectionString))
                    {
                        SqlConnection.Open();
                        foreach (var menu in menuModels)
                        {
                            using (SqlCommand cmd = new SqlCommand("UPDATE [dbo].[LinkGroup] SET IsSelected=@IsSelected WHERE MenuID =@MenuID and GroupID=@GroupID", SqlConnection))
                            {
                                cmd.Parameters.AddWithValue("@MenuID", menu.MenuID);
                                cmd.Parameters.AddWithValue("@GroupID", GroupID);
                                cmd.Parameters.AddWithValue("@IsSelected", menu.IsSelected);

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}