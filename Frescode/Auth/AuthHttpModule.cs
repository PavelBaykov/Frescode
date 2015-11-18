using System;
using System.Web;
using Microsoft.Practices.ServiceLocation;

namespace Frescode.Auth
{
    public class AuthHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += Authenticate;
        }

        private static void Authenticate(object source, EventArgs e)
        {
            var app = (HttpApplication)source;
            var context = app.Context;

            var auth = ServiceLocator.Current.GetInstance<IAuthentication>();
            auth.HttpContext = context;
            context.User = auth.CurrentUser;
        }

        public void Dispose()
        {
        }
    }
}
