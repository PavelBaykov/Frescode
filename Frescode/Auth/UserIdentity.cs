using System.Linq;
using System.Security.Principal;
using Frescode.DAL;
using Frescode.DAL.Entities;

namespace Frescode.Auth
{
    public class UserIndentity : IIdentity
    {
        private readonly RootContext _rootContext;

        public UserIndentity(RootContext rootContext)
        {
            _rootContext = rootContext;
        }

        public User User { get; set; }

        public string AuthenticationType => typeof(User).ToString();

        public bool IsAuthenticated => User != null;

        public string Name
        {
            get
            {
                if (User != null)
                {
                    return User.UserName;
                }
                return "anonym";
            }
        }

        public void Init(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                User = _rootContext.Users.Single(x => x.UserName == userName);
            }
        }
    }
}
