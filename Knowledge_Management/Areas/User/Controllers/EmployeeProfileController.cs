using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Knowledge_Management.DAL;
using System.Web.Security;


namespace Knowledge_Management.Areas.User.Controllers
{
    [CustomAuthorize(Roles = "Public")]
    public class EmployeeProfileController : Controller
    {
        // GET: EmployeeProfile
        public ActionResult Index()
        {
           return View();
        }
    }
}