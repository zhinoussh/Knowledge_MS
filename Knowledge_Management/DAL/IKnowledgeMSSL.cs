using Knowledge_Management.Areas.Admin.ViewModels;
using Knowledge_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knowledge_Management.DAL
{
    public interface IKnowledgeMSSL
    {
        IKnowledgeMSDAL DataLayer { get;set; }

        #region HomeController

        string Post_Login(string userName, string passWord, Controller ctrl);

        #endregion HomeController

        #region DepartmentController

        void Post_Add_Edit_Department(DepartmentViewModel vm);
        DepartmentViewModel Get_Delete_Department(int departmentId);
        void Post_Delete_Department(DepartmentViewModel vm);
        List<tbl_department> Get_DepartmentTableContent(string filter, string sortDirection, int displayStart, int displayLength);

        #endregion DepartmentController

    }
}