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
    [CustomAuthorize(Roles = "DataView")]
    public class SearchKeyController : Controller
    {
        // GET: SearchKey
        public ActionResult Index()
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

            int jobDesc_id =Request["jobdesc_id"].ToString()=="" ? 0 : Convert.ToInt32(Request["jobdesc_id"].ToString());
            int depObj_id = Request["depObj_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["depObj_id"].ToString());
            int st_id = Request["st_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["st_id"].ToString());

            List<SearchKeywordViewModel> all_items = DAL.get_Keywords(jobDesc_id, depObj_id,st_id);

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

    }
}