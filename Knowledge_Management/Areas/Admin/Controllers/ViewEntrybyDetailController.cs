using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Knowledge_Management.Models;
using Knowledge_Management.DAL;
using Knowledge_Management.Areas.Admin.ViewModels;
using Knowledge_Management.Areas.User.ViewModels;


namespace Knowledge_Management.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class ViewEntrybyDetailController : Controller
    {
        // GET: ViewEntrybyDetail
        public ActionResult Index()
        {
            DetailQuestionViewModel o = new DetailQuestionViewModel();
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();


            List<tbl_department> deps = DAL.get_all_Departments();
            o.lst_dep = new SelectList(deps, "pkey", "department_name");
            o.dep_id = deps.First().pkey;

            List<tbl_job> jobs = DAL.get_Jobs(o.dep_id);
            o.lst_job = new SelectList(jobs, "pkey", "job_name");
            o.job_id = jobs.First().pkey ;


            List<tbl_department_objectives> dep_objs = DAL.get_Department_Objectives(o.dep_id);
            o.lst_dep_objective = new SelectList(dep_objs, "pkey", "objective");
            o.dep_obj_id = 0;

            List<tbl_strategy> strategies = DAL.get_all_strategies();
            o.lst_strategy = new SelectList(strategies, "pkey", "strategy_name");
            o.strategy_id = 0;

            List<tbl_job_description> jobDescs = DAL.get_JobDescriptions(o.job_id);
            o.lst_job_desc = new SelectList(jobDescs, "pkey", "job_desc");
            o.job_desc_id = 0;

            return View(o);

        }

        public JsonResult FillObjectives(int DepId)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            List<tbl_department_objectives> objectives = DAL.get_Department_Objectives(DepId);

            List<SelectListItem> lst_obj = new List<SelectListItem>();
            lst_obj.Add(new SelectListItem { Value = "0", Text = "----" });

            foreach (tbl_department_objectives o in objectives)
            {
                lst_obj.Add(new SelectListItem { Value = o.pkey + "", Text = o.objective });
            }

            return Json(lst_obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FillJobDescriptions(int JobId)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            List<tbl_job_description> desc = DAL.get_JobDescriptions(JobId);

            List<SelectListItem> lst_obj = new List<SelectListItem>();
            lst_obj.Add(new SelectListItem { Value = "0", Text ="----" });

            foreach (tbl_job_description jd in desc)
            {
                lst_obj.Add(new SelectListItem { Value = jd.pkey + "", Text = jd.job_desc });
            }

            return Json(lst_obj, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SearchQuestionAjaxHandler(jQueryDataTableParamModel request)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            List<QuestionViewModel> all_items = new List<QuestionViewModel>();

            int jobDesc_id = Request["jobDesc_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["jobDesc_id"].ToString());
            int depObj_id = Request["depObj_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["depObj_id"].ToString());
            int strategy_id = Request["strategy_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["strategy_id"].ToString());


            all_items = DAL.get_all_Questions_by_details(jobDesc_id, depObj_id,strategy_id);
            

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
            DetailQuestionViewModel q = new DetailQuestionViewModel();
            q.question_id = id;
            return PartialView("_PartialDeleteQuestion", q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Question(DetailQuestionViewModel q)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                DAL.DeleteQuestion(q.question_id);
                return Json(new { msg = "مسئله با موفقیت Delete شد" });
            }
            else
            {
                ModelState.AddModelError("Delete_Question", "error in deleting Question");
            }
            return View(q);

        }

    }
}