using System.Web.Mvc;
using System.Web.Routing;

namespace Frescode
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "nestedDefectSpotItem",
               url: "User/{userId}/Project/{projectId}/Checklist/{checklistId}/Details/{checklistItemId}/DefectSpot/{defectSpotId}",
               defaults: new { controller = "ChecklistItems", action = "DefectSpotAddition" }
            );
            routes.MapRoute(
               name: "defectSpotBreadcrumb",
               url: "User/{userId}/Project/{projectId}/Checklist/{checklistId}/Details/{checklistItemId}/DefectSpot/{defectSpotId}/GetBreadcrumbText",
               defaults: new { controller = "ChecklistItems", action = "GetBreadcrumbDefectSpotText" }
            );

            routes.MapRoute(
               name: "nestedChecklistItem",
               url: "User/{userId}/Project/{projectId}/Checklist/{checklistId}/Details/{checklistItemId}",
               defaults: new { controller = "ChecklistItems", action = "ChecklistItemDetails" }
            );
            routes.MapRoute(
               name: "checklistItemBreadcrumb",
               url: "User/{userId}/Project/{projectId}/Checklist/{checklistId}/Details/{checklistItemId}/GetBreadcrumbText",
               defaults: new { controller = "ChecklistItems", action = "GetBreadcrumbText" }
            );


            routes.MapRoute(
               name: "nestedChecklistItemsList",
               url: "User/{userId}/Project/{projectId}/Checklist/{checklistId}",
               defaults: new { controller = "ChecklistItems", action = "ChecklistItemsList" }
            );
            routes.MapRoute(
               name: "ChecklistItemsListBreadcrumb",
               url: "User/{userId}/Project/{projectId}/Checklist/{checklistId}/GetBreadcrumbText",
               defaults: new { controller = "Checklist", action = "GetBreadcrumbText" }
            );


            routes.MapRoute(
               name: "nestedChecklistsList",
               url: "User/{userId}/Project/{projectId}",
               defaults: new { controller = "Checklist", action = "ChecklistsList" }
            );
            routes.MapRoute(
               name: "ChecklistsListBreadcrumb",
               url: "User/{userId}/Project/{projectId}/GetBreadcrumbText",
               defaults: new { controller = "Project", action = "GetBreadcrumbText" }
            );


            routes.MapRoute(
               name: "nestedProjectsList",
               url: "User/{userId}",
               defaults: new { controller = "Project", action = "ProjectsList" }
            );
            routes.MapRoute(
               name: "ProjectsListBreadcrumb",
               url: "User/{userId}/GetBreadcrumbText",
               defaults: new { controller = "User", action = "GetBreadcrumbText" }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
