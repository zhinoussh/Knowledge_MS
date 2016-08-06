using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Knowledge_Management.DAL;
using System.Web.Security;
using Knowledge_Management.App_Code;

namespace Knowledge_Management.Controllers
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