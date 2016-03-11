using System.Web.Mvc;
using System.Web.Routing;

namespace Frescode
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //TODO refactoring!!!

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "nestedDefectionDownloading",
               url: "Project/{projectId}/Checklist/{checklistId}/Details/{checklistItemId}/DefectSpot/{defectSpotId}/UploadFiles",
               defaults: new { controller = "DefectSpotPicture", action = "GetFiles" },
               constraints: new { httpMethod = new HttpMethodConstraint("GET")}
            );

            routes.MapRoute(
               name: "nestedDefectionUploadin",
               url: "Project/{projectId}/Checklist/{checklistId}/Details/{checklistItemId}/DefectSpot/{defectSpotId}/UploadFiles",
               defaults: new { controller = "DefectSpotPicture", action = "UploadFiles" },
               constraints: new { httpMethod = new HttpMethodConstraint("POST")}
            );

            routes.MapRoute(
               name: "nestedDefectSpotItem",
               url: "Project/{projectId}/Checklist/{checklistId}/Details/{checklistItemId}/DefectSpot/{defectSpotId}",
               defaults: new { controller = "ChecklistItems", action = "DefectSpotAddition" }
            );
            routes.MapRoute(
               name: "defectSpotBreadcrumb",
               url: "Project/{projectId}/Checklist/{checklistId}/Details/{checklistItemId}/DefectSpot/{defectSpotId}/GetBreadcrumbText",
               defaults: new { controller = "ChecklistItems", action = "GetBreadcrumbDefectSpotText" }
            );

            routes.MapRoute(
               name: "nestedChecklistItem",
               url: "Project/{projectId}/Checklist/{checklistId}/Details/{checklistItemId}",
               defaults: new { controller = "ChecklistItems", action = "ChecklistItemDetails" }
            );
            routes.MapRoute(
               name: "checklistItemBreadcrumb",
               url: "Project/{projectId}/Checklist/{checklistId}/Details/{checklistItemId}/GetBreadcrumbText",
               defaults: new { controller = "ChecklistItems", action = "GetBreadcrumbText" }
            );


            routes.MapRoute(
               name: "nestedChecklistItemsList",
               url: "Project/{projectId}/Checklist/{checklistId}",
               defaults: new { controller = "ChecklistItems", action = "ChecklistItemsList" }
            );
            routes.MapRoute(
               name: "ChecklistItemsListBreadcrumb",
               url: "Project/{projectId}/Checklist/{checklistId}/GetBreadcrumbText",
               defaults: new { controller = "Checklist", action = "GetBreadcrumbText" }
            );

           routes.MapRoute(
              name: "nestedInspectionDrawingScreen",
              url: "Project/{projectId}/InspectionDrawing/{drawingId}",
              defaults: new { controller = "InspectionDrawingScreen", action = "InspectionDrawingsDetails" }
           );

            routes.MapRoute(
              name: "InspectionDrawingBreadcrumb",
              url: "Project/{projectId}/InspectionDrawing/{drawingId}/GetBreadcrumbText",
              defaults: new { controller = "InspectionDrawingScreen", action = "GetBreadcrumbText" }
           );


            routes.MapRoute(
               name: "nestedChecklistsList",
               url: "Project/{projectId}",
               defaults: new { controller = "ProjectScreen", action = "ProjectScreenList" }
            );

            routes.MapRoute(
               name: "ChecklistsListBreadcrumb",
               url: "Project/{projectId}/GetBreadcrumbText",
               defaults: new { controller = "Project", action = "GetBreadcrumbText" }
            );

            routes.MapRoute(
               name: "ProjectsList",
               url: "GetProjectsList",
               defaults: new { controller = "Project", action = "GetProjectsList" }
            );

            routes.MapRoute(
               name: "ProjectsListBreadcrumb",
               url: "GetBreadcrumbText",
               defaults: new { controller = "User", action = "GetBreadcrumbText" }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Project", action = "ProjectsList", id = UrlParameter.Optional }
            );

        }
    }
}
