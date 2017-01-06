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
    public class ViewEntrybyEmployeeController : Controller
    {
        IKnowledgeMSSL serviceLayer;

        public ViewEntrybyEmployeeController(IKnowledgeMSSL service)
        {
            serviceLayer = service;
        }

        #region Personel
        // GET: ViewEntrybyEmployee
        public ActionResult Personel()
        {
            return View();
        }

        public ActionResult EmployeeAjaxHandler(jQueryDataTableParamModel request)
        {
            Tuple<List<Employee>, int> tbl_content = serviceLayer.Get_EmployeeTableContent(request.sSearch, Convert.ToInt32(Request["iSortCol_0"])
                                                , Request["sSortDir_0"], request.iDisplayStart, request.iDisplayLength);

            var indexed_list = tbl_content.Item1.Select((s, index) => new
            {
                SID = s.Emp_Id + "",
                SIndex = (index + 1) + "",
                FNAME = s.Emp_fname,
                LNAME = s.Emp_lname,
                Pcode = s.Emp_pcode,
                DepName = s.Dep_Name,
                Jobname = s.Job_Name,
                Dt_Entry = s.data_entry + "",
                DT_View = s.data_view + ""
            });

            var result = from s in indexed_list
                         select new[] { s.SID, s.SIndex, s.FNAME, s.LNAME, s.Pcode
                             , s.DepName, s.Jobname,s.Dt_Entry,s.DT_View };

            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = tbl_content.Item2,
                iTotalDisplayRecords = tbl_content.Item2,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        #endregion Personel

        #region Question

        [ActionName("Question")]
        public ActionResult Question_byEmployee(int id)
        {
            EmployeeQuestionViewModel vm = serviceLayer.Get_Question_Index_Page(id);
            return View(vm);
        }

        public ActionResult SearchQuestionAjaxHandler(jQueryDataTableParamModel request)
        {
            int emp_id = Request["emp_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["emp_id"].ToString());

           Tuple<List<QuestionViewModel>, int> tbl_content= serviceLayer.Get_QuestionbyEmployeeTableContent(emp_id, request.sSearch, Request["sSortDir_0"], request.iDisplayStart, request.iDisplayLength);

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
                         select new[] { s.QID, s.KeyWords, s.Job_Desc, s.Dep_Obj, s.Strategy, s.QSubject, s.QIndex, s.QSubject.Length <= 200 ? s.QSubject : (s.QSubject.Substring(0, 200) + "...") };



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
        public ActionResult Delete_Question(int id)
        {
            EmployeeQuestionViewModel q = serviceLayer.Get_Delete_QuestionbyEmployee(id);
            return PartialView("_PartialDeleteQuestion", q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Question(EmployeeQuestionViewModel q)
        {
            if (ModelState.IsValid)
            {
                serviceLayer.Post_Delete_QuestionbyEmployee(q);
                return Json(new { msg = "Question deleted Successfully." });
            }
            else
            {
                ModelState.AddModelError("Delete_Question", "error in deleting Question");
            }
            return View(q);

        }
        
        [HttpGet] 
        public ActionResult QuestionDetails(QuestionViewModel vm)
        {
            return PartialView("_PartialQuestionDetail", vm);
        }

        #endregion Question

        #region Solution

        public ActionResult QuestionSolutions(int id)
        {
            SolutionViewModel vm = serviceLayer.Get_QuestionSolutions_Index_Page(id);
            return View(vm);
        }

        public ActionResult EmployeeSolutions(int id)
        {
            EmployeeQuestionViewModel vm = serviceLayer.Get_EmployeeSolutions_Index_Page(id);
            return View(vm);
        }

        public ActionResult SolutionQuestionAjaxHandler(jQueryDataTableParamModel request)
        {
            int question_id = Request.QueryString["q_id"].ToString() == "" ? 0 : Int32.Parse(Request.QueryString["q_id"].ToString());

            Tuple<List<SolutionEmployeeViewModel>, int> tbl_content = serviceLayer.Get_SolutionForQuestionTableContent(question_id,0, request.sSearch, Request["sSortDir_0"]
                  , request.iDisplayStart, request.iDisplayLength);
           
            var indexed_list = tbl_content.Item1.Select((s, index) => new
            {
                SID = s.solution_id + "",
                FullSolution = s.solution,
                Confirm=s.confirm.ToString(),
                SIndex = (index + 1) + ""
                ,Uploads=s.count_upload+""
            });

            var result = from s in indexed_list
                         select new[] { s.SID,  s.SIndex
                             ,s.FullSolution.Length <= 200 ? s.FullSolution : (s.FullSolution.Substring(0, 200) + "...")
                            ,s.Confirm,s.Uploads};

            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = tbl_content.Item2,
                iTotalDisplayRecords =tbl_content.Item2,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        public ActionResult SolutionEmployeeAjaxHandler(jQueryDataTableParamModel request)
        {
            int employee_id = Request.QueryString["emp_id"].ToString() == "" ? 0 : Int32.Parse(Request.QueryString["emp_id"].ToString());

             Tuple<List<SolutionEmployeeViewModel>, int> tbl_content=serviceLayer.Get_SolutionForEmployeeTableContent(employee_id,
                 request.sSearch,Request["sSortDir_0"],request.iDisplayStart,request.iDisplayLength);

             var indexed_list = tbl_content.Item1.Select((s, index) => new
            {
                SID = s.solution_id + "",
                Question=s.question,
                FullSolution = s.solution,
                Confirm = s.confirm.ToString(),
                SIndex = (index + 1) + ""
                ,
                Uploads = s.count_upload + ""
            });
            
            var result = from s in indexed_list
                         select new[] { s.SID,  s.SIndex
                              ,s.Question.Length <= 200 ? s.Question : (s.Question.Substring(0, 200) + "...")
                             ,s.FullSolution.Length <= 200 ? s.FullSolution : (s.FullSolution.Substring(0, 200) + "...")
                              ,s.Confirm,s.Uploads};

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
        public ActionResult Delete_Solution(int id)
        {
            SolutionEmployeeViewModel q = serviceLayer.Get_Delete_Solution(id);
            return PartialView("_PartialDeleteSolution", q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValidator]
        public ActionResult Delete_Solution(SolutionEmployeeViewModel q)
        {
            serviceLayer.Post_Delete_Solution(q, this);
            return Json(new { msg = "Solution deleted successfully" });   
        }

        public ActionResult ViewFullSolution(int id)
        {
            FullSolutionViewModel vm = serviceLayer.Get_FullSolution(id);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Confirm_Solution_Employee(int s_id, int emp_id)
        {
            EmployeeQuestionViewModel vm=serviceLayer.Post_Confirm_Solution_for_Employee(s_id, emp_id);
            return View("EmployeeSolutions", vm);
        }

        [HttpPost]
        public ActionResult Confirm_Solution_Question(int s_id, int q_id)
        {
            SolutionViewModel vm = serviceLayer.Post_Confirm_Solution_for_Question(s_id, q_id);
            return View("QuestionSolutions", vm);
        }

        #endregion Solution

        #region UPLOAD FILES

        [HttpGet] 
        public ActionResult Delete_Upload(int id)
        {
            UploadViewModel q = serviceLayer.Get_Delete_Upload(id);
            return PartialView("_PartialDeleteUpload", q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ModelValidator]
        public ActionResult Delete_Upload(UploadViewModel q)
        {
            serviceLayer.Post_Delete_Upload(q, this);
            return Json(new { msg = "File deleted successfully" });
        }

        public ActionResult UploadAjaxHandler(jQueryDataTableParamModel request)
        {
            long solution_id = Convert.ToInt64(Request["solution_id"].ToString());
            Tuple<List<tbl_solution_uploads>, int> tbl_content = serviceLayer.Get_UploadForSolutionTableContent(
                solution_id,0, request.iDisplayStart, request.iDisplayLength);

            var indexed_list = tbl_content.Item1.Select((s, index) => new { SID = s.pkey + "", FILEPATH = s.file_path, SIndex = (index + 1) + "", SNAME = s.file_desc, Confirm = s.confirm.ToString() });

            var result = from s in indexed_list
                         select new[] { s.SID, s.FILEPATH, s.SIndex, s.SNAME ,s.Confirm};

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
            Tuple<byte[], string> file_prop=serviceLayer.GetFilePropertie(uploadID, this);
            return File(file_prop.Item1, System.Net.Mime.MediaTypeNames.Application.Octet, file_prop.Item2);
        }
        
        [HttpPost]
        public ActionResult Confirm_Upload(int u_id,int s_id)
        {
           FullSolutionViewModel vm = serviceLayer.Post_Confirm_Upload(u_id, s_id);           
           return View("ViewFullSolution",vm);
        }

        #endregion UPLOAD FILES
    }
}