using System.Security.Principal;
using Frescode.DAL;

namespace Frescode.Auth
{
    public class UserProvider : IPrincipal
    {
        private UserIndentity UserIdentity { get; }

        #region IPrincipal Members

        public IIdentity Identity => UserIdentity;

        public bool IsInRole(string role)
        {
            if (UserIdentity.User == null)
            {
                return false;
            }
            return UserIdentity.User.UserRole.ToString().Equals(role);
        }

        #endregion


        public UserProvider(string name, RootContext rootContext)
        {
            UserIdentity = new UserIndentity(rootContext);
            UserIdentity.Init(name);
        }


        public override string ToString()
        {
            return UserIdentity.Name;
        }
    }
}
