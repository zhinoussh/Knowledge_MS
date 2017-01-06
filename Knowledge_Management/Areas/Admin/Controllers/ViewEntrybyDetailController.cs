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
        IKnowledgeMSSL serviceLayer;

        public ViewEntrybyDetailController(IKnowledgeMSSL service)
        {
            serviceLayer = service;
        }
       
        // GET: ViewEntrybyDetail
        public ActionResult Index()
        {
            DetailQuestionViewModel o = serviceLayer.Get_EntrybyDetail_Index_Page();
            return View(o);
        }

        public JsonResult FillObjectives(int DepId)
        {
            List<SelectListItem> objectives = serviceLayer.GetDepartmentObjectioveList(DepId);
            return Json(objectives, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FillJobDescriptions(int JobId)
        {
            List<SelectListItem> lst_obj = serviceLayer.GetJobDescriptionList(JobId);
            return Json(lst_obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchQuestionAjaxHandler(jQueryDataTableParamModel request)
        {

            int jobDesc_id = Request["jobDesc_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["jobDesc_id"].ToString());
            int depObj_id = Request["depObj_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["depObj_id"].ToString());
            int strategy_id = Request["strategy_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["strategy_id"].ToString());

            Tuple<List<QuestionViewModel>, int> tbl_content = serviceLayer.Get_QuestionbyDetailTableContent(depObj_id, jobDesc_id, strategy_id, request.sSearch
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
                QWriter = s.emp_prop
            });


            var result = from s in indexed_list
                         select new[] { s.QID, s.KeyWords, s.Job_Desc, s.Dep_Obj, s.Strategy, s.QSubject,s.QWriter, s.QIndex, s.QSubject.Length <= 200 ? s.QSubject : (s.QSubject.Substring(0, 200) + "...") };



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


        [HttpGet] 
        public ActionResult Delete_Question(int id)
        {
            DetailQuestionViewModel q = serviceLayer.Get_Delete_QuestionbyDetail(id);
            return PartialView("_PartialDeleteQuestion", q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Question(DetailQuestionViewModel q)
        {
            if (ModelState.IsValid)
            {
                serviceLayer.Post_Delete_QuestionbyDetail(q);
                return Json(new { msg = "Question deleted Successfully." });
            }
            else
            {
                ModelState.AddModelError("Delete_Question", "error in deleting Question");
            }
            return View(q);

        }

    }
}