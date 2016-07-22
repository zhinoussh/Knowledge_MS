﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Knowledge_Management.ViewModels;
using Knowledge_Management.DAL;
using System.Web.Security;

namespace Knowledge_Management.Controllers
{
    public class SolutionController : Controller
    {

        public ActionResult Solutions(int id)
        {
            SolutionViewModel o = new SolutionViewModel();
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            o.question_id = id;
            o.solutions = DAL.get_Solutions(id);
            o.question = DAL.get_question_name(id);
            o.new_solution = "";
            return View(o);
        }

        //create a Question
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_New_Solution(SolutionViewModel q)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
                HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
                string UserName = ticket.Name; //You have the UserName!

                //List<string> lst_keywords = new List<string>();
                //int count_keywords = Int32.Parse(Request["count"].ToString());
                //for (int i = 1; i <= count_keywords; i++)
                //{
                //    if (Request["field" + i] != null)
                //        lst_keywords.Add(Request["field" + i].ToString());
                //}


                DAL.InsertNewSolution(q.question_id, q.new_solution, UserName);
                return Json(new { msg = "راهکار با موفقیت ذخیره شد", url = "/Solution/Solutions/" + q.question_id });
            }

            return View(q);
        }

    }
}