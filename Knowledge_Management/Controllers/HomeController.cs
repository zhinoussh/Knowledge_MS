using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Knowledge_Management.DAL;
using Knowledge_Management.Models;
using System.Web.Security;

namespace Knowledge_Management.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private IKnowledgeMSSL serviceLayer;

        public HomeController(IKnowledgeMSSL service)
        {
            serviceLayer = service;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValidator]
        public ActionResult Index(tbl_login user)
        {
            string returnUrl = serviceLayer.Post_Login(user.username, user.pass, this);

            if(!String.IsNullOrEmpty(returnUrl))
                return Json(new { url = returnUrl });
            else
                return PartialView("_LoginPartial", user);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }
        
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}