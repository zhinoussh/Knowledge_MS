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
        IKnowledgeMSSL serviceLayer;
        public InsertInfoController(IKnowledgeMSSL service)
        {
            serviceLayer = service;
        }

        // GET: InsertInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewQuestion(int? id)
        {
            int q_id=id.HasValue ? id.Value : 0;
            QuestionViewModel o = serviceLayer.Get_NewQuestion_Page(q_id, this);

            return View(o);
        }

        //create a Question
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValidator]
        public ActionResult Add_Edit_Question(QuestionViewModel q)
        {
            serviceLayer.Post_Add_Edit_Question(q, this);
            return Json(new { msg = "Question added Successfully." });
        }

        [HttpGet] 
        public ActionResult Delete_Question(int id)
        {
            QuestionViewModel q = serviceLayer.Get_Delete_Question(id);
            return PartialView("_PartialDeleteQuestion", q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValidator]
        public ActionResult Delete_Question(QuestionViewModel q)
        {
            serviceLayer.Post_Delete_Question(q);
            return Json(new { msg = "Question deleted Successfully." });
        }

        public ActionResult YourQuestionAjaxHandler(jQueryDataTableParamModel request)
        {
            Tuple<List<QuestionViewModel>, int> tbl_content=serviceLayer.Get_UserQuestionsTableContent(this, request.sSearch, Request["sSortDir_0"], request.iDisplayStart, request.iDisplayLength);
            
            var indexed_list = tbl_content.Item1.Select((s, index) => new
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
                iTotalRecords = tbl_content.Item2,
                iTotalDisplayRecords = tbl_content.Item2,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

    }
}