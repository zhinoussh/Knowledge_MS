using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knowledge_Management.DAL
{
    public class AuthSL : IAuthSL
    {
        private IKnowledgeMSDAL _dataLayer;

        public IKnowledgeMSDAL DataLayer
        {
            get
            {
                if (_dataLayer == null)
                    _dataLayer = new KnowledgeMSDAL(new KnowledgeMsDB());

                return _dataLayer;
            }
        }

        public void InitialiseAdmin()
        {
            if (!DataLayer.check_userinRole("admin", "Admin"))
            {
                string encrypt_pass = (new Encryption()).Encrypt("1");
                DataLayer.initialise_admin_user(encrypt_pass);
            }
        }

        public string[] Get_User_Roles(string username)
        {
            return DataLayer.get_user_roles(username);
        }

        public bool IsUserInRole(string UserName, string Role)
        {
            return DataLayer.check_userinRole(UserName, Role);
        }

   
    }
}