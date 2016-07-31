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

        public ActionResult NewSolution(int id)
        {
            SolutionViewModel o = new SolutionViewModel();
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            o.question_id = id;
             o.question = DAL.get_question_name(id);
             o.new_solution = "";
             o.new_solution_id =0;

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
        public ActionResult Add_New_Solution(SolutionViewModel q)
        {
            if (ModelState.IsValid)
            {
                KnowledgeMSDAL DAL = new KnowledgeMSDAL();

                string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
                HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
                string UserName = ticket.Name; //You have the UserName!

                long new_id=DAL.InsertNewSolution(q.question_id, q.new_solution, UserName);
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


        #region UPLOAD FILES
       
        public ActionResult Upload()
        {
            KnowledgeMSDAL DAL = new KnowledgeMSDAL();
            // string solution_id = Request.QueryString["id"].ToString();
            //check soution exist if not insert one
            long new_id = Request.Form["solution_id"] == null ? 0 : long.Parse(Request.Form["solution_id"].ToString());
            
            if (new_id == 0)
            {
                string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
                HttpCookie authCookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); //Decrypt it
                string UserName = ticket.Name; //You have the UserName!

                new_id = DAL.InsertNewSolution(long.Parse(Request.Form["question_id"] == null ? "0" : Request.Form["question_id"])
                    , "بدون شرح راهکار", UserName);

            }

            //upload file
            var file = Request.Files["Filedata"];
            var fileNameExt = file.FileName.Substring(file.FileName.LastIndexOf('.'));

            string file_name = new_id + "_" + (DAL.get_count_solution_uploads(new_id) + 1) + fileNameExt;
            string savePath = Server.MapPath(@"~\Upload\" + file_name);
            file.SaveAs(savePath);

            //save upload in DB
            DAL.InsertNewUpload(new_id, file_name);
           

            return Content(new_id+"");
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

                DAL.DeleteQuestion(q.upload_id);
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
    }
}