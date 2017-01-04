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
        IKnowledgeMSSL serviceLayer;
        public SearchInfoController(IKnowledgeMSSL service)
        {
            serviceLayer = service;
        }
        // GET: SearchInfo
        public ActionResult SearchAll(int?id)
        {
            ViewBag.key_id =( id.HasValue ? id.Value : 0);
            
            return View();
        }
        
        public ActionResult SearchAllQuestionAjaxHandler(jQueryDataTableParamModel request)
        {
            int key_id = Request["key_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["key_id"].ToString());

         Tuple<List<QuestionViewModel>, int> tbl_content=serviceLayer.Get_AllQuestionsTableContent(key_id, request.sSearch, Request["sSortDir_0"]
                , request.iDisplayStart, request.iDisplayLength);
            

         var indexed_list = tbl_content.Item1.Select((s, index) => new
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

        // GET: SearchKey
        public ActionResult SearchbyKeyword()
        {
            SearchKeywordViewModel o = serviceLayer.Get_SearchKeyword_Page(this);
            return View(o);
        }

        public ActionResult KeywordAjaxHandler(jQueryDataTableParamModel request)
        {

            int jobDesc_id = Request["jobdesc_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["jobdesc_id"].ToString());
            int depObj_id = Request["depObj_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["depObj_id"].ToString());
            int st_id = Request["st_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["st_id"].ToString());

           Tuple<List<SearchKeywordViewModel>, int> tbl_content= serviceLayer.Get_KeywordTableContent(depObj_id, jobDesc_id, st_id, request.sSearch, Request["sSortDir_0"]
                , request.iDisplayStart, request.iDisplayLength);

           var indexed_list = tbl_content.Item1.Select((s, index) => new
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
                iTotalRecords = tbl_content.Item2,
                iTotalDisplayRecords = tbl_content.Item2,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult KeywordDetails(KeywordDetailViewModel vm)
        {
            vm = serviceLayer.Get_KeywordDetails(vm);
            return PartialView("_PartialDetailKeyWord", vm);
        }
    }
}