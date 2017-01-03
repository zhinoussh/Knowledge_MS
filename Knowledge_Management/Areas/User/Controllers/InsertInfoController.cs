using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Knowledge_Management.Models;
using Knowledge_Management.DAL;
using System.Web.Security;
using Knowledge_Management.Areas.User.ViewModels;



namespace Knowledge_Management.Areas.User.Controllers
{
    [CustomAuthorize(Roles = "DataEntry")]
    public class InsertInfoController : Controller
    {
        // GET: InsertInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewQuestion(int? id)
        {
            QuestionViewModel o = new QuestionViewModel();
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
            HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
            string UserName = ticket.Name; //You have the UserName!

            List<string> emp_prop = DAL.get_Employee_prop(UserName);
            int dep_id = Int32.Parse(emp_prop[0]);
            int job_id = Int32.Parse(emp_prop[1]);


            List<tbl_department_objectives> dep_objs = DAL.get_Department_Objectives(dep_id);
            o.lst_dep_objective = new SelectList(dep_objs, "pkey", "objective");

            List<tbl_strategy> strategies = DAL.get_all_strategies();
            o.lst_strategy = new SelectList(strategies, "pkey", "strategy_name");

            List<tbl_job_description> jobDescs = DAL.get_JobDescriptions(job_id);
            o.lst_job_desc = new SelectList(jobDescs, "pkey", "job_desc");

            //insert
            if (!id.HasValue)
            {
                o.question_id=0;
                o.question = "";
                o.lst_keywords = "";
                o.dep_obj_id = 0;
                o.strategy_id = 0;
                o.job_desc_id = 0;
            }
            //edit
            else
            {
                QuestionViewModel question=DAL.get_question_byId(id.Value);
                o.question_id = id.Value;
                o.question = question.question;
                o.lst_keywords = question.lst_keywords;
                o.strategy_id = question.strategy_id.HasValue ? question.strategy_id.Value : 0;
                o.dep_obj_id = question.dep_obj_id.HasValue ? question.dep_obj_id.Value : 0;
                o.job_desc_id = question.job_desc_id.HasValue ? question.job_desc_id.Value : 0;
            }
            return View(o);
        }

        //create a Question
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_Edit_Question(QuestionViewModel q)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
                HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
                string UserName = ticket.Name; //You have the UserName!

                List<string> lst_keywords = new List<string>();
                int count_keywords=Int32.Parse(Request["count"].ToString());
                for(int i=1;i<=count_keywords;i++)
                {
                    if(Request["field"+i]!=null)
                        lst_keywords.Add(Request["field"+i].ToString());
                }

                
                DAL.InsertQuestion(q.question_id, q.question, q.dep_obj_id, q.job_desc_id, q.strategy_id, lst_keywords, UserName);
                return Json(new { msg = "Question added Successfully." });
            }
            else
            {
                ModelState.AddModelError("ADD_QuestionErr", "Question length is exeeding");
            }
            return View(q);
        }


        [HttpGet] // this action result returns the partial containing the modal
        public ActionResult Delete_Question(int id)
        {
            QuestionViewModel q = new QuestionViewModel();
            q.question_id = id;
            return PartialView("_PartialDeleteQuestion", q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Question(QuestionViewModel q)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                DAL.DeleteQuestion(q.question_id);
                return Json(new { msg = "Question deleted Successfully." });
            }
            else
            {
                ModelState.AddModelError("Delete_Question", "error in deleting Question");
            }
            return View(q);

        }

        public ActionResult QuestionAjaxHandler(jQueryDataTableParamModel request)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
            HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
            string UserName = ticket.Name; //You have the UserName!

            List<QuestionViewModel> all_items = DAL.get_all_Questions_by_employee(UserName);

            //filtering 
            List<QuestionViewModel> filtered = new List<QuestionViewModel>();

            if (!string.IsNullOrEmpty(request.sSearch))
            {
                filtered = all_items.Where(i => i.question.Contains(request.sSearch)).ToList();

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
                QIndex = (index + 1) + "",
                QSubject = s.question,               
                DepObjId = s.dep_obj_id + "",
                JobDescId = s.job_desc_id + "",
                StId = s.strategy_id + "",
                KeyWords = s.lst_keywords
            });

            var result = from s in indexed_list
                         select new[] { s.QID, s.DepObjId, s.JobDescId, s.StId, s.KeyWords, s.QIndex, s.QSubject };



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