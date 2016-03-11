using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DALLib;
using MediatR;
using DALLib.Entities;

namespace Frescode.Controllers
{
    [Authorize]
    public class ChecklistController : BaseController
    {
        public ChecklistController(IMediator mediator, RootContext rootContext)
            : base(mediator, rootContext)
        {
            
        }

        [HttpGet]
        public async Task<ActionResult> GetBreadcrumbText(int checklistId)
        {
            var checklist = await Context.ChecklistsForProject
                .Include(x => x.ChecklistTemplate)
                .SingleOrDefaultAsync(x => x.Id == checklistId);
            return Json(new {Text = checklist?.ChecklistTemplate?.Name}, JsonRequestBehavior.AllowGet);
        }

    }
}