using Knowledge_Management.DAL;
using Knowledge_Management.Helpers;
using Knowledge_Management.Models;
using Knowledge_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Knowledge_Management.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class ViewEntrybyJobController : Controller
    {
        // GET: ViewEntrybyJob
        public ActionResult Index()
        {
            JobDepQuestionViewModel o = new JobDepQuestionViewModel();
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            List<tbl_department> deps = DAL.get_all_Departments();
            o.lst_dep = new SelectList(deps, "pkey", "department_name");
            o.dep_id = deps.First().pkey + "";

            List<tbl_job> jobs = DAL.get_Jobs(Int32.Parse(o.dep_id));
            o.lst_job = new SelectList(jobs, "pkey", "job_name");
            o.job_id = "0";

            return View(o);
        }

        #region Question

        public JsonResult FillJobs(int DepId)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            List<tbl_job> jobs = DAL.get_Jobs(DepId);

            List<SelectListItem> lst_obj = new List<SelectListItem>();
            lst_obj.Add(new SelectListItem { Value =  "0", Text ="همه مشاغل" });

            foreach (tbl_job j in jobs)
            {
                lst_obj.Add(new SelectListItem { Value = j.pkey + "", Text = j.job_name });
            }

            return Json(lst_obj, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult SearchQuestionAjaxHandler(jQueryDataTableParamModel request)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            List<QuestionViewModel> all_items = new List<QuestionViewModel>();

            int job_id = Request["job_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["job_id"].ToString());
            int dep_id = Request["dep_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["dep_id"].ToString());

            if (job_id != 0)
            {
                all_items = DAL.get_all_Questions_by_job(job_id);
            }
            else if(dep_id!=0)
                all_items = DAL.get_all_Questions_by_alljobs_department(dep_id);


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
                         select new[] { s.QID, s.KeyWords, s.Job_Desc, s.Dep_Obj, s.Strategy, s.QSubject, s.QIndex, s.QSubject.Length <= 200 ? s.QSubject : (s.QSubject.Substring(0, 200) + "...") };



            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = all_items.Count(),
                iTotalDisplayRecords = all_items.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }


        [HttpGet] // this action result returns the partial containing the modal
        public ActionResult Delete_Question(int id)
        {
            JobDepQuestionViewModel q = new JobDepQuestionViewModel();
            q.question_id = id;
            return PartialView("_PartialDeleteQuestion", q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Question(JobDepQuestionViewModel q)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                DAL.DeleteQuestion(q.question_id);
                return Json(new { msg = "مسئله با موفقیت حذف شد" });
            }
            else
            {
                ModelState.AddModelError("Delete_Question", "error in deleting Question");
            }
            return View(q);

        }

        #endregion Question
    }
}