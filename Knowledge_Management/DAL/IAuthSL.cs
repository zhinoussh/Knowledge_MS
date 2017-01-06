using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knowledge_Management.DAL
{
    public interface IAuthSL
    {
        IKnowledgeMSDAL DataLayer{get;}
        void InitialiseAdmin();

        string[] Get_User_Roles(string username);

        bool IsUserInRole(string UserName, string Role);
    }
}
