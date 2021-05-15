using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserManagementApp.Models;
using UserManagementApp.Service;


namespace UserManagementApp.Controllers
{
    public class ReportController : Controller
    {

        private readonly IUserManagement _userManagement;

        public ReportController(IUserManagement userManagement)
        {
            _userManagement = userManagement;
        }
        // GET: Report
        public ActionResult UserReport()
        {
            return View();
        }

        public ActionResult UserList()
        {
            try
            {
                if (HttpContext.Session["Userdetails"] != null)
                {
                    IEnumerable<UserModel> response = _userManagement.GetAllUsers();
                    return Json(new { success = true, resultData = response }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, resultData = "Session Expired" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, resultData = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}