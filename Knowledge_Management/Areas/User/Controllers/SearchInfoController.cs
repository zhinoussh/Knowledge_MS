using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Knowledge_Management.DAL;
using Knowledge_Management.Areas.User.ViewModels;
using System.Web.Security;
using System.Web;
using Knowledge_Management.Models;



namespace Knowledge_Management.Areas.User.Controllers
{
    [CustomAuthorize(Roles = "DataView")]
    public class SearchInfoController : Controller
    {
        // GET: SearchInfo
        public ActionResult SearchAll(int?id)
        {
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
                         select new[] { s.QID, s.KeyWords, s.Job_Desc, s.Dep_Obj, s.Strategy, s.QSubject, s.QIndex, s.QSubject.Length <= 200 ? s.QSubject : (s.QSubject.Substring(0, 200)+"...") };



            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = all_items.Count(),
                iTotalDisplayRecords = all_items.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult QuestionDetails(QuestionViewModel vm)
        {
            return PartialView("_PartialQuestionDetail", vm);
        }

        // GET: SearchKey
        public ActionResult SearchbyKeyword()
        {
            SearchKeywordViewModel o = new SearchKeywordViewModel();
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
            o.dep_obj_id = 0;

            List<tbl_strategy> strategies = DAL.get_all_strategies();
            o.lst_strategy = new SelectList(strategies, "pkey", "strategy_name");
            o.strategy_id = 0;

            List<tbl_job_description> jobDescs = DAL.get_JobDescriptions(job_id);
            o.lst_job_desc = new SelectList(jobDescs, "pkey", "job_desc");
            o.job_desc_id = 0;


            return View(o);
        }

        public ActionResult KeywordAjaxHandler(jQueryDataTableParamModel request)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            int jobDesc_id = Request["jobdesc_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["jobdesc_id"].ToString());
            int depObj_id = Request["depObj_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["depObj_id"].ToString());
            int st_id = Request["st_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["st_id"].ToString());

            List<SearchKeywordViewModel> all_items = DAL.get_Keywords(jobDesc_id, depObj_id, st_id);

            //filtering 
            List<SearchKeywordViewModel> filtered = new List<SearchKeywordViewModel>();

            if (!string.IsNullOrEmpty(request.sSearch))
            {
                filtered = all_items.Where(i => i.keyword.Contains(request.sSearch)).ToList();

            }
            else
                filtered = all_items;


            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filtered = filtered.OrderBy(s => s.keyword).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.keyword).ToList();

            //pagination
            filtered = filtered.Skip(request.iDisplayStart).Take(request.iDisplayLength).ToList();

            var indexed_list = filtered.Select((s, index) => new
            {
                KID = s.key_id + "",
                KIndex = (index + 1) + "",
                Keyword = s.keyword,
                DepObjId = s.dep_obj_id + "",
                JobDescId = s.job_desc_id + "",
                StId = s.strategy_id + "",
            });

            var result = from s in indexed_list
                         select new[] { s.KID, s.DepObjId, s.JobDescId, s.StId, s.KIndex, s.Keyword };



            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = all_items.Count(),
                iTotalDisplayRecords = all_items.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult KeywordDetails(KeywordDetailViewModel vm)
        {
            KnowledgeMSDAL db = new KnowledgeMSDAL();
            vm.job_desc = db.get_job_description(vm.jobDescId);
            vm.dep_obj = db.get_department_objective(vm.depObjId);
            vm.strategy = db.get_strategy_description(vm.strategyId);

            return PartialView("_PartialDetailKeyWord", vm);
        }
    }
}