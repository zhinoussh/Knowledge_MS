using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Knowledge_Management.DAL;
using Microsoft.Practices.Unity;
using Knowledge_Management.Helpers;

namespace Knowledge_Management
{
    public class CustomRoleProvider:RoleProvider
    {
        private IUnityContainer unityResolver;

        public CustomRoleProvider()
        {
            unityResolver = Bootstrapper.Initialise();

        }
        
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            var myAuthService = unityResolver.Resolve<IAuthSL>();
            return myAuthService.Get_User_Roles(username);
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var myAuthService = unityResolver.Resolve<IAuthSL>();
            return myAuthService.IsUserInRole(username, roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}