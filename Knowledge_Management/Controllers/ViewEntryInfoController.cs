using Knowledge_Management.DAL;
using Knowledge_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Knowledge_Management.ViewModels;

namespace Knowledge_Management.Controllers
{
    public class ViewEntryInfoController : Controller
    {
        #region Personel
        // GET: ViewEntryInfo
        public ActionResult Personel()
        {
            return View();
        }

        public ActionResult EmployeeAjaxHandler(jQueryDataTableParamModel request)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            List<Employee> all_items = DAL.get_Employees();

            //filtering 
            List<Employee> filtered = new List<Employee>();

            if (!string.IsNullOrEmpty(request.sSearch))
            {
                filtered = all_items.Where(i => i.Emp_fname.Contains(request.sSearch)
                                        || i.Emp_lname.Contains(request.sSearch)
                                        || i.Emp_pcode.Contains(request.sSearch)
                                        || i.Dep_Name.Contains(request.sSearch)
                                        || i.Job_Name.Contains(request.sSearch)
                    ).ToList();
            }
            else
                filtered = all_items;


            //sorting
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Employee, string> orderingFunction = (c => sortColumnIndex == 2 ? c.Emp_fname :
                                                        sortColumnIndex == 3 ? c.Emp_lname :
                                                        sortColumnIndex == 4 ? c.Emp_pcode :
                                                        sortColumnIndex == 5 ? c.Dep_Name :
                                                         sortColumnIndex == 6 ? c.Dep_Name : "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filtered = filtered.OrderBy(orderingFunction).ToList();
            else
                filtered = filtered.OrderByDescending(orderingFunction).ToList();

            //pagination
            filtered = filtered.Skip(request.iDisplayStart).Take(request.iDisplayLength).ToList();

            var indexed_list = filtered.Select((s, index) => new
            {
                SID = s.Emp_Id + "",
                SIndex = (index + 1) + ""
                ,
                FNAME = s.Emp_fname,
                LNAME = s.Emp_lname,
                Pcode = s.Emp_pcode,
                DepName = s.Dep_Name,
                DepId = s.Dep_Id + ""
                ,
                Jobname = s.Job_Name,
                JobId = s.Job_Id + "",
                Dt_Entry = s.data_entry + "",
                DT_View = s.data_view + ""
            });

            var result = from s in indexed_list
                         select new[] { s.SID, s.SIndex, s.FNAME, s.LNAME, s.Pcode
                             , s.DepName, s.Jobname,s.Dt_Entry,s.DT_View };



            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = all_items.Count(),
                iTotalDisplayRecords = all_items.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        #endregion Personel

        #region Question
        public ActionResult Question(int pId)
        {
            KnowledgeMSDAL dal=new KnowledgeMSDAL();
            ViewQuestionViewModel vm = new ViewQuestionViewModel();
            if (pId != 0)
            {
                List<string> emp_props=dal.get_Employee_byId(pId);
                vm.emp_id = pId;
                vm.Description = "مطرح شده توسط آقای/خانم  " + emp_props[1] + " کد پرسنلی: " + emp_props[0];
            }


            return View(vm);
        }

        public ActionResult SearchQuestionAjaxHandler(jQueryDataTableParamModel request)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            List<QuestionViewModel> all_items = new List<QuestionViewModel>();

            int emp_id = Request["emp_id"].ToString() == "" ? 0 : Convert.ToInt32(Request["emp_id"].ToString());

            if (emp_id != 0)
            {
                List<string> emp_props = DAL.get_Employee_byId(emp_id);
                all_items = DAL.get_all_Questions_by_employee(emp_props[0]);
            }

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
            ViewQuestionViewModel q = new ViewQuestionViewModel();
            q.question_id = id;
            return PartialView("_PartialDeleteQuestion", q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Question(ViewQuestionViewModel q)
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

        #region Solution

        public ActionResult SolutionList(int id)
        {
            KnowledgeMSDAL dal=new KnowledgeMSDAL();
            SolutionViewModel vm = new SolutionViewModel();
            if (id != 0)
            {
                vm.question = dal.get_question_name(id);
                vm.question_id = id;              
            }

            return View(vm);
        }

        public ActionResult SoutionListAjaxHandler(jQueryDataTableParamModel request)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            int question_id = Request.QueryString["q_id"].ToString() == "" ? 0 : Int32.Parse(Request.QueryString["q_id"].ToString());

            List<SolutionEmployeeViewModel> all_items = DAL.get_Solutions_by_Question(question_id);

            //filtering 
            List<SolutionEmployeeViewModel> filtered = new List<SolutionEmployeeViewModel>();

            if (!string.IsNullOrEmpty(request.sSearch))
            {
                filtered = all_items.Where(i => i.solution.Contains(request.sSearch)).ToList();

            }
            else
                filtered = all_items;


            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filtered = filtered.OrderBy(s => s.solution).ToList();
            else
                filtered = filtered.OrderByDescending(s => s.solution).ToList();

            //pagination
            filtered = filtered.Skip(request.iDisplayStart).Take(request.iDisplayLength).ToList();

            var indexed_list = filtered.Select((s, index) => new
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
                iTotalRecords = all_items.Count(),
                iTotalDisplayRecords = all_items.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }


        [HttpGet] // this action result returns the partial containing the modal
        public ActionResult Delete_Solution(int id)
        {
            SolutionEmployeeViewModel q = new SolutionEmployeeViewModel();
            q.solution_id = id;
            return PartialView("_PartialDeleteSolution", q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Solution(SolutionEmployeeViewModel q)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();
                long id_solution = q.solution_id;

                List<tbl_solution_uploads> lst_uploads = DAL.get_uploads_by_solution(id_solution);
                //delete uploaded files
                foreach (var item in lst_uploads)
                {
                    System.IO.File.Delete(Server.MapPath(@"~/Upload/" + item.file_path));
                }

                //delete solution and upload from db
                DAL.Delete_Solution(id_solution);

                return Json(new { msg = "راهکار مورد نظر با موفقیت حذف شد" });
            }
            else
            {
                ModelState.AddModelError("Delete_Solution", "error in deleting Solution");
            }
            return View(q);

        }

        public ActionResult ViewFullSolution(int id)
        {
            KnowledgeMSDAL DAL=new KnowledgeMSDAL();
            FullSolutionViewModel vm = new FullSolutionViewModel();
            vm= DAL.get_Solution_by_id(id);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Confirm_Solution(int s_id,int q_id)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            SolutionViewModel vm = new SolutionViewModel();
            if (q_id != 0)
            {
                vm.question = DAL.get_question_name(q_id);
                vm.question_id = q_id;
            }
            DAL.change_confirm_status_solution(s_id);
            return View("SolutionList",vm);
        }


        #endregion Solution


        #region UPLOAD FILES


        [HttpGet] // this action result returns the partial containing the modal
        public ActionResult Delete_Upload(int id)
        {
            UploadViewModel q = new UploadViewModel();
            q.upload_id = id;
            return PartialView("_PartialDeleteUpload", q);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Upload(UploadViewModel q)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                System.IO.File.Delete(Server.MapPath(@"~\Upload\" + DAL.get_file_path(q.upload_id)));

                DAL.DeleteUpload(q.upload_id);
                return Json(new { msg = "فایل بارگذاری شده با موفقیت حذف شد" });
            }
            else
            {
                ModelState.AddModelError("Delete_Upload", "error in deleting Upload");
            }
            return View(q);

        }

        public ActionResult UploadAjaxHandler(jQueryDataTableParamModel request)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            long solution_id = Convert.ToInt32(Request["solution_id"].ToString());

            List<tbl_solution_uploads> filtered = DAL.get_uploads_by_solution(solution_id);


            //pagination
            filtered = filtered.Skip(request.iDisplayStart).Take(request.iDisplayLength).ToList();

            var indexed_list = filtered.Select((s, index) => new { SID = s.pkey + "", FILEPATH = s.file_path, SIndex = (index + 1) + "", SNAME = "فایل " + (index + 1),Confirm=s.confirm.ToString() });

            var result = from s in indexed_list
                         select new[] { s.SID, s.FILEPATH, s.SIndex, s.SNAME ,s.Confirm};


            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = filtered.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }


        public ActionResult DownloadFile(int uploadID)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            string file_name = DAL.get_file_path(uploadID);
            string file_path = Server.MapPath(@"~\Upload\" + file_name);
            byte[] fileBytes = System.IO.File.ReadAllBytes(file_path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file_name);
        }

        
       [HttpPost]
        public ActionResult Confirm_Upload(int u_id,int s_id)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            DAL.change_confirm_status_upload(u_id);
          
           FullSolutionViewModel vm = new FullSolutionViewModel();
            vm = DAL.get_Solution_by_id(s_id);
           
           return View("ViewFullSolution",vm);
        }


        #endregion UPLOAD FILES
    }
}