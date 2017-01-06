using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Knowledge_Management.DAL;
using System.Web.Security;
using Knowledge_Management.Models;
using Knowledge_Management.Areas.User.ViewModels;



namespace Knowledge_Management.Areas.User.Controllers
{
    [CustomAuthorize(Roles = "DataView")]
    public class SolutionController : Controller
    {
        IKnowledgeMSSL serviceLayer;
        public SolutionController(IKnowledgeMSSL service)
        {
            serviceLayer = service;
        }

        public ActionResult Index(int id)
        {
            SolutionViewModel o = serviceLayer.Get_Solution_Index_Page(id);
            return View(o);
        }


        public ActionResult SolutionsForQuestionAjaxHandler(jQueryDataTableParamModel request)
        {
            int question_id = Request.QueryString["q_id"].ToString() == "" ? 0 : Int32.Parse(Request.QueryString["q_id"].ToString());

            Tuple<List<SolutionEmployeeViewModel>, int> tbl_content=serviceLayer.Get_SolutionForQuestionTableContent(question_id, 1, request.sSearch, Request["sSortDir_0"]
                , request.iDisplayStart, request.iDisplayLength);


            var indexed_list = tbl_content.Item1.Select((s, index) => new
            {
                SID = s.solution_id + "",
                FullSolution = s.solution,
                SIndex = (index + 1) + ""
                ,Uploads=s.count_upload+""
            });

            var result = from s in indexed_list
                         select new[] { s.SID,  s.SIndex
                             ,s.FullSolution.Length <= 200 ? s.FullSolution : (s.FullSolution.Substring(0, 200) + "...")
                            ,s.Uploads};

            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = tbl_content.Item2,
                iTotalDisplayRecords = tbl_content.Item2,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewSolution(int id,long? solution_id)
        {
            long solutionId = solution_id.HasValue ? solution_id.Value : 0;
            NewSolutionViewModel o = serviceLayer.Get_NewSolution_Page(id, solutionId);           
            return View(o);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValidator]
        public ActionResult Add_New_Solution(NewSolutionViewModel q)
        {
            long new_id = serviceLayer.Post_Add_New_Solution(q, this);
            return Json(new { msg = "Solution Added Successfully.", new_solution_id = new_id });
        }

        public ActionResult FullSolution(int id)
        {
            FullSolutionViewModel s = serviceLayer.Get_FullSolution(id);
            return View(s);
        }

        [HttpGet]
        public ActionResult Delete_Solution(int id)
        {
            SolutionEmployeeViewModel v = serviceLayer.Get_Delete_Solution(id);
            return PartialView("_PartialDeleteSolution",v);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Solution(SolutionEmployeeViewModel s)
        {
            serviceLayer.Post_Delete_Solution(s, this);
            return Json(new { msg="Solution Deleted Successfully."});
        }

        #region UPLOAD FILES

        [ValidateAntiForgeryToken]
        public ActionResult Upload()
        {
            long solution_id = Request.Form["solution_id"] == null ? 0 : long.Parse(Request.Form["solution_id"].ToString());
            long question_id=Request.Form["question_id"] == null ? 0: long.Parse(Request.Form["question_id"].ToString());
            string upload_desc=Request.Form["upload_desc"] == null ? "":  Request["upload_desc"].ToString();

            long new_solution_id = serviceLayer.Upload_File(question_id, solution_id, upload_desc, this);

            return Content(new_solution_id + "");
        }

        [HttpGet] // this action result returns the partial containing the modal
        public ActionResult Delete_Upload(int id)
        {
            UploadViewModel q = serviceLayer.Get_Delete_Upload(id);
            return PartialView("_PartialDeleteUpload", q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValidator]
        public ActionResult Delete_Upload(UploadViewModel vm)
        {
            serviceLayer.Post_Delete_Upload(vm, this);
            return Json(new { msg = "File Uploaded Successfully." });
        }

        public ActionResult UploadAjaxHandler(jQueryDataTableParamModel request)
        {
            long solution_id = Convert.ToInt64(Request["solution_id"].ToString());

            Tuple<List<tbl_solution_uploads>, int> tbl_content = serviceLayer.Get_UploadForSolutionTableContent(
                solution_id,1, request.iDisplayStart, request.iDisplayLength);

            var indexed_list = tbl_content.Item1.Select((s, index) => new { SID = s.pkey + "", FILEPATH = s.file_path, SIndex = (index + 1) + "", SNAME = s.file_desc });

            var result = from s in indexed_list
                         select new[] { s.SID, s.FILEPATH, s.SIndex, s.SNAME };


            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = tbl_content.Item2,
                iTotalDisplayRecords = tbl_content.Item2,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadAjaxHandler_NewSOlution(jQueryDataTableParamModel request)
        {
          
            long solution_id = Convert.ToInt64(Request["solution_id"].ToString());

            Tuple<List<tbl_solution_uploads>, int> tbl_content = serviceLayer.Get_UploadForSolutionTableContent(
                solution_id, 0, request.iDisplayStart, request.iDisplayLength);

            var indexed_list = tbl_content.Item1.Select((s, index) => new
            {
                SID = s.pkey + "",
                FILEPATH = s.file_path
                , SIndex = (index + 1) + "", SNAME = s.file_desc ,Confirm=s.confirm+""});

            var result = from s in indexed_list
                         select new[] { s.SID, s.FILEPATH, s.SIndex, s.SNAME,s.Confirm };

          
            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = tbl_content.Item2,
                iTotalDisplayRecords = tbl_content.Item2,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadFile(int uploadID)
        {
            Tuple<byte[],string> file_prop =serviceLayer.GetFilePropertie(uploadID, this);
            return File(file_prop.Item1, System.Net.Mime.MediaTypeNames.Application.Octet, file_prop.Item2);
        }

        public ActionResult EditUpload(NewSolutionViewModel vm)
        {
            return PartialView("_PartialEditUpload",vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_Upload_description(NewSolutionViewModel vm)
        {
            serviceLayer.Get_Edit_Upload_description(vm);
            return Json(new { msg = "Upload description changed Successfully" });
        }
    
        #endregion UPLOAD FILES

        #region Your Solution

        public ActionResult YourSolution()
        {
            return View();
        }
        
        public ActionResult YourSolutionAjaxHandler(jQueryDataTableParamModel request)
        {
            Tuple <List < SolutionEmployeeViewModel >,int> tbl_content = serviceLayer.Get_UserSolutionsTableContent(this, request.sSearch
                , Request["sSortDir_0"], request.iDisplayStart, request.iDisplayLength);

            var indexed_list = tbl_content.Item1.Select((s, index) => new
            {
                QID = s.question_id + "",
                Sol_Id = s.solution_id + "",
                Solution = s.solution,
                QIndex = (index + 1) + "",
                QSubject = s.question,
                uploadCount=s.count_upload+"",
                confirm_status=s.confirm+""
            });

            var result = from s in indexed_list
                         select new[] {s.Sol_Id, s.QID , s.QIndex
                             ,s.QSubject.Length <= 200 ? s.QSubject: (s.QSubject.Substring(0, 200) + "..."), 
                                 s.Solution.Length <= 200 ? s.Solution : (s.Solution.Substring(0, 200) + "..."),
                                 s.uploadCount,s.confirm_status
                         };

            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = tbl_content.Item2,
                iTotalDisplayRecords = tbl_content.Item2,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);

        }

        #endregion Your Solution
    }
}