using Knowledge_Management.Areas.Admin.ViewModels;
using Knowledge_Management.Areas.User.ViewModels;
using Knowledge_Management.DAL;
using Knowledge_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Knowledge_Management.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class ViewEntrybyJobController : Controller
    {
        IKnowledgeMSSL serviceLayer;

        public ViewEntrybyJobController(IKnowledgeMSSL service)
        {
            serviceLayer = service;
        }

        // GET: ViewEntrybyJob
        public ActionResult Index()
        {
            JobDepQuestionViewModel o = serviceLayer.Get_EntrybyJob_Index_Page();
            return View(o);
        }


        public JsonResult FillJobs(int DepId)
        {
            List<SelectListItem> lst_obj = serviceLayer.GetJobList(DepId);
            return Json(lst_obj, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult SearchQuestionAjaxHandler(jQueryDataTableParamModel request)
        {

            int job_id = Request["job_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["job_id"].ToString());
            int dep_id = Request["dep_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["dep_id"].ToString());

            Tuple<List<QuestionViewModel>, int> tbl_content =serviceLayer.Get_QuestionbyJobTableContent(dep_id, job_id, request.sSearch
                , Request["sSortDir_0"], request.iDisplayStart, request.iDisplayLength);

            var indexed_list = tbl_content.Item1.Select((s, index) => new
            {
                QID = s.question_id + "",
                KeyWords = s.lst_keywords,
                Dep_Obj = s.dep_objective,
                Strategy = s.strategy_name,
                Job_Desc = s.job_desc,
                QIndex = (index + 1) + "",
                QSubject = s.question,
                QWriter=s.emp_prop
            });

            var result = from s in indexed_list
                         select new[] { s.QID, s.KeyWords, s.Job_Desc, s.Dep_Obj, s.Strategy, s.QSubject,s.QWriter, s.QIndex
                             , s.QSubject.Length <= 200 ? s.QSubject : (s.QSubject.Substring(0, 200) + "...") };

            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = tbl_content.Item2,
                iTotalDisplayRecords = tbl_content.Item2,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult QuestionDetails(QuestionViewModel vm)
        {
            return PartialView("_PartialQuestionDetail", vm);
        }

        [HttpGet] // this action result returns the partial containing the modal
        public ActionResult Delete_Question(int id)
        {
            JobDepQuestionViewModel q = serviceLayer.Get_Delete_QuestionbyJob(id);
            return PartialView("_PartialDeleteQuestion", q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Question(JobDepQuestionViewModel q)
        {
            if (ModelState.IsValid)
            {
                serviceLayer.Post_Delete_QuestionbyJob(q);
                return Json(new { msg = "Question deleted successfully" });
            }
            else
            {
                ModelState.AddModelError("Delete_Question", "error in deleting Question");
            }
            return View(q);

        }

    }
}