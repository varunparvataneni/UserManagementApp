using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserManagementApp.Models;
using UserManagementApp.Service;

namespace UserManagementApp.Controllers
{
    public class GroupController : Controller
    {

        private readonly IUserManagement _userManagement;

        public GroupController(IUserManagement userManagement)
        {
            _userManagement = userManagement;
        }
        // GET: Group
        public ActionResult Index()
        {
            if (HttpContext.Session["Userdetails"] != null)
            {
                IEnumerable<GroupModel> groupDetails = _userManagement.GetAllGroups();

                return View(groupDetails);
            }
            return RedirectToAction("DashBoard", "Home");
        }

      
        // GET: Group/Create
        public ActionResult Create()
        {
            if (HttpContext.Session["Userdetails"] != null)
            {
                GroupModel groupModel = new GroupModel();
                groupModel.MenuList = _userManagement.GetMenuItems().ToList();
                return View(groupModel);
            }
            return RedirectToAction("DashBoard", "Home");
        }

        // POST: Group/Create
        [HttpPost]
        public ActionResult Create(GroupModel groupModel)
        {
            try
            {
                if (HttpContext.Session["Userdetails"] != null)
                {

                    if (ModelState.IsValid)
                    {
                        var groupDetails = _userManagement.SubmitGroup(groupModel);
                        _userManagement.LinkGroups(groupDetails.GroupID, groupModel.MenuList,"Create");
                        return RedirectToAction("Index");
                    }
                    return View("Create", groupModel);
                }
                return RedirectToAction("DashBoard", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: Group/Edit/5
        public ActionResult Edit(int id)
        {
            if (HttpContext.Session["Userdetails"] != null)
            {
                GroupModel groupModel = _userManagement.GetGroupDetails(id);
                return View(groupModel);
            }
            return RedirectToAction("DashBoard", "Home");
        }

        // POST: Group/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, GroupModel groupModel)
        {
            try
            {
                if (HttpContext.Session["Userdetails"] != null)
                {
                    _userManagement.LinkGroups(id, groupModel.MenuList, "Edit");

                    return RedirectToAction("Index");
                }
                return RedirectToAction("DashBoard", "Home");
            }
            catch
            {
                return View();
            }
        }
    }
}
