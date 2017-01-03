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

    }
}