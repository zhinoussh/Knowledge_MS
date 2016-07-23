using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Knowledge_Management.Models;
using Knowledge_Management.ViewModels;
using Knowledge_Management.DAL;
using System.Web.Security;

namespace Knowledge_Management.Controllers
{
    public class SearchInfoController : Controller
    {
        // GET: SearchInfo
        public ActionResult SearchAll(int?id)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
            HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
            string UserName = ticket.Name; //You have the UserName!

            List<string> emp_prop = DAL.get_Employee_prop(UserName);

            ViewBag.dataentry = Boolean.Parse(emp_prop[2]);
            ViewBag.dataview = Boolean.Parse(emp_prop[3]);


            ViewBag.key_id = id == null ? 0 : id;
            
            return View();
        }

       
        
        public ActionResult SearchQuestionAjaxHandler(jQueryDataTableParamModel request)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            List<QuestionViewModel> all_items = new List<QuestionViewModel>();
            
            long key_id = Request["key_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["key_id"].ToString());
            
            if(key_id==0)
                all_items = DAL.get_all_Questions();
            else
                all_items = DAL.get_all_Questionsby_key(key_id);

            //filtering 
            List<QuestionViewModel> filtered = new List<QuestionViewModel>();

            if (!string.IsNullOrEmpty(request.sSearch))
            {
                filtered = all_items.Where(i => i.question.Contains(request.sSearch)
                                             || i.lst_keywords.Contains(request.sSearch)).ToList();

            }
            else
                filtered = all_items;


            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filtered = filtered.OrderBy(s => s.question).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.question).ToList();

            //pagination
            filtered = filtered.Skip(request.iDisplayStart).Take(request.iDisplayLength).ToList();

            var indexed_list = filtered.Select((s, index) => new
            {
                QID = s.question_id + "",                               
                KeyWords = s.lst_keywords,
                Dep_Obj = s.dep_objective,
                Strategy = s.strategy_name,
                Job_Desc = s.job_desc,
                QIndex = (index + 1) + "",
                QSubject = s.question
            });

            var result = from s in indexed_list
                         select new[] { s.QID, s.KeyWords, s.Job_Desc, s.Dep_Obj,s.Strategy, s.QIndex, s.QSubject };



            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = all_items.Count(),
                iTotalDisplayRecords = all_items.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }


      
    }
}