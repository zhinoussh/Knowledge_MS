using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Knowledge_Management.DAL;
using System.Web.Security;
using Knowledge_Management.Areas.User.ViewModels;


namespace Knowledge_Management.Areas.User.Controllers
{
    [CustomAuthorize(Roles = "Public,DataView,DataEntry")]
    public class EmployeeProfileController : Controller
    {
        IKnowledgeMSSL serviceLayer;

        public EmployeeProfileController(IKnowledgeMSSL service)
        {
            serviceLayer = service;
        }
        public ActionResult Index()
        {
            ProfileViewModel vm=serviceLayer.Get_Profile(this);
           return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProfile(ProfileViewModel vm)
        {
            serviceLayer.Post_Change_Profile(vm,this);
            return Json(new { msg = "your profile changed successfully" });
        }
    }
}