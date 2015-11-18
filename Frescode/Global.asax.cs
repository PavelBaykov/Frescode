using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Autofac.Features.Variance;
using Autofac.Integration.Mvc;
using Frescode.Auth;
using Frescode.DAL;
using MediatR;
using Microsoft.Practices.ServiceLocation;

namespace Frescode
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Handler")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();
            builder.RegisterType<RootContext>().AsSelf();
            //builder.RegisterType<CustomAuthentication>().AsImplementedInterfaces();

            builder.RegisterSource(new ContravariantRegistrationSource());
            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            var csl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => csl);
        }
    }
}
