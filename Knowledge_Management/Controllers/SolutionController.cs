using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Knowledge_Management.ViewModels;
using Knowledge_Management.DAL;
using System.Web.Security;
using Knowledge_Management.Models;

namespace Knowledge_Management.Controllers
{
    [Authorize(Roles = "Employee")]
    public class SolutionController : Controller
    {

        public ActionResult Index(int id)
        {
            SolutionViewModel o = new SolutionViewModel();
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            o.question_id = id;
            o.solutions = DAL.get_Solutions(id);
            o.question = DAL.get_question_name(id);
           
            string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
            HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
            string UserName = ticket.Name; //You have the UserName!

            List<string> emp_prop = DAL.get_Employee_prop(UserName);

            ViewBag.dataentry = Boolean.Parse(emp_prop[2]);
            ViewBag.dataview = Boolean.Parse(emp_prop[3]);

            return View(o);
        }

        public ActionResult NewSolution(int id,long? solution_id)
        {
            NewSolutionViewModel o = new NewSolutionViewModel();
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            o.question_id = id;

            long pk_solution = long.Parse(solution_id == null ? "0" : solution_id+"");
            if (solution_id != null)
            {
                FullSolutionViewModel full_solution = DAL.get_Solution_by_id(pk_solution);
                o.question = full_solution.question;
                o.new_solution = full_solution.full_solution;
            }
            else
            {
                o.question = DAL.get_question_name(id);
                o.new_solution = "";
            }


            o.new_solution_id = pk_solution;

            string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
            HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
            string UserName = ticket.Name; //You have the UserName!

            List<string> emp_prop = DAL.get_Employee_prop(UserName);

            ViewBag.dataentry = Boolean.Parse(emp_prop[2]);
            ViewBag.dataview = Boolean.Parse(emp_prop[3]);
            

            return View(o);
        }

        //create a Question
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_New_Solution(NewSolutionViewModel q)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
                HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
                string UserName = ticket.Name; //You have the UserName!

                long new_id=DAL.InsertNewSolution(q.new_solution_id,q.question_id, q.new_solution, UserName);
                q.new_solution_id = new_id;
                
                return Json(new { msg = "راهکار با موفقیت ذخیره شد" });
            }

            return View(q);
        }

        public ActionResult FullSolution(int id)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
            HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
            string UserName = ticket.Name; //You have the UserName!

            List<string> emp_prop = DAL.get_Employee_prop(UserName);

            ViewBag.dataentry = Boolean.Parse(emp_prop[2]);
            ViewBag.dataview = Boolean.Parse(emp_prop[3]);

            FullSolutionViewModel s = new FullSolutionViewModel();
            s= DAL.get_Solution_by_id(id);            

            return View(s);
        }

        [HttpGet]
        public ActionResult Delete_Solution(long id)
        {
            SolutionEmployeeViewModel v = new SolutionEmployeeViewModel();
            v.solution_id = id;
            return PartialView("_PartialDeleteSolution",v);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_Solution(SolutionEmployeeViewModel s)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            long id_solution = s.solution_id;
           
             List<tbl_solution_uploads> lst_uploads=DAL.get_uploads_by_solution(id_solution);
            //delete uploaded files
             foreach (var item in lst_uploads)
            { 
                System.IO.File.Delete(Server.MapPath(@"~/Upload/"+item.file_path));
            }

            //delete solution and upload from db
            DAL.Delete_Solution(id_solution);
            
            return Json(new { msg="راهکار مورد نظر با موفقیت حذف شد"});

        }

        #region UPLOAD FILES
       
        public ActionResult Upload()
        {
            long new_id = Request.Form["solution_id"] == null ? 0 : long.Parse(Request.Form["solution_id"].ToString());

            if (Request.Form["question_id"] != null)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();
                // string solution_id = Request.QueryString["id"].ToString();
                //check soution exist if not insert one

                if (new_id == 0)
                {
                    string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
                    HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
                    string UserName = ticket.Name; //You have the UserName!


                    new_id = DAL.InsertNewSolution(new_id, long.Parse(Request.Form["question_id"]), "بدون شرح راهکار", UserName);

                }

                //upload file
                var file = Request.Files["Filedata"];
                var fileNameExt = file.FileName.Substring(file.FileName.LastIndexOf('.'));

                string file_name = new_id + "_" + (DAL.get_count_solution_uploads(new_id) + 1) + fileNameExt;
                string savePath = Server.MapPath(@"~\Upload\" + file_name);
                file.SaveAs(savePath);

                //save upload in DB
                DAL.InsertNewUpload(new_id, file_name);


            }
            return Content(new_id + "");

        }

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


            //filtering 
           // List<tbl_solution_uploads> filtered = new List<tbl_solution_uploads>();

            //if (!string.IsNullOrEmpty(request.sSearch))
            //{
            //    filtered = all_items.Where(i => i.strategy_name.Contains(request.sSearch)).ToList();

            //}
            //else
            //    filtered = all_items;
           
            //var sortDirection = Request["sSortDir_0"]; // asc or desc
            //if (sortDirection == "asc")
            //    filtered = filtered.OrderBy(s => s.strategy_name).ToList();
            //else
            //    filtered = filtered.OrderByDescending(s => s.strategy_name).ToList();

            //pagination
            filtered = filtered.Skip(request.iDisplayStart).Take(request.iDisplayLength).ToList();

            var indexed_list = filtered.Select((s, index) => new { SID = s.pkey + "",FILEPATH=s.file_path, SIndex = (index + 1) + "", SNAME = "فایل " + (index + 1) });

            var result = from s in indexed_list
                         select new[] { s.SID, s.FILEPATH, s.SIndex, s.SNAME };

          
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

            string file_name=DAL.get_file_path(uploadID);
            string file_path = Server.MapPath(@"~\Upload\" + file_name);
            byte[] fileBytes = System.IO.File.ReadAllBytes(file_path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file_name);
        }

        #endregion UPLOAD FILES

        #region Your Solution

        public ActionResult YourSolution()
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
            HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
            string UserName = ticket.Name; //You have the UserName!

            List<string> emp_prop = DAL.get_Employee_prop(UserName);

            ViewBag.dataentry = Boolean.Parse(emp_prop[2]);
            ViewBag.dataview = Boolean.Parse(emp_prop[3]);

            return View();
        }
        public ActionResult YourSolutionAjaxHandler(jQueryDataTableParamModel request)
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();

            string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
            HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
            string UserName = ticket.Name; //You have the UserName!

            List<SolutionEmployeeViewModel> all_items = DAL.get_Solutions_by_employee(UserName);

            //filtering 
            List<SolutionEmployeeViewModel> filtered = new List<SolutionEmployeeViewModel>(); ;

            if (!string.IsNullOrEmpty(request.sSearch))
            {
                filtered = all_items.Where(i => i.question.Contains(request.sSearch)
                                             || i.solution.Contains(request.sSearch)).ToList();

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
                Sol_Id = s.solution_id + "",
                Soution = s.solution,
                QIndex = (index + 1) + "",
                QSubject = s.question,
                uploadCount=s.count_upload+""
            });


            var result = from s in indexed_list
                         select new[] {s.Sol_Id, s.QID , s.QSubject, s.Soution, s.QIndex
                             ,s.QSubject.Length <= 50 ? s.QSubject: (s.QSubject.Substring(0, 50) + "..."), 
                                 s.Soution.Length <= 50 ? s.Soution : (s.Soution.Substring(0, 50) + "..."),
                                 s.uploadCount
                         };


            return Json(new
            {
                sEcho = request.sEcho,
                iTotalRecords = all_items.Count(),
                iTotalDisplayRecords = all_items.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);

        }

        #endregion Your Solution
    }
}