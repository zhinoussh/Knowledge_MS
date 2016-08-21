using Knowledge_Management.DAL;
using Knowledge_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                         select new[] { s.SID, s.DepId, s.JobId, s.SIndex, s.FNAME, s.LNAME, s.Pcode
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
            if(pId!=null)

            return View();
        }

        #endregion Question



    }
}