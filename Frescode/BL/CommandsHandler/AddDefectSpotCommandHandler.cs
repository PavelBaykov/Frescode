using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Frescode.BL.Commands;
using DALLib;
using DALLib.Entities;
using MediatR;

namespace Frescode.BL.CommandsHandler
{
    public class AddDefectSpotCommandHandler : IAsyncNotificationHandler<AddDefectSpotCommand>
    {
        private readonly RootContext _rootContext;

        public AddDefectSpotCommandHandler(RootContext rootContext)
        {
            _rootContext = rootContext;
        }

        public async Task Handle(AddDefectSpotCommand notification)
        {
            DefectionSpot defectionSpot;
            if (notification.Id <= 0)
            {
                defectionSpot = new DefectionSpot
                {
                    DateCreated = DateTime.Now,
                    AttachedPictures = new List<Picture>(),
                    ChecklistItem = await _rootContext.ChecklistItems.SingleOrDefaultAsync(c => c.Id == notification.ChecklistItemId)
                };
                //_rootContext.DefectionSpots.Add(defectionSpot);
            }
            else
            {
                //defectionSpot = await _rootContext.DefectionSpots.SingleOrDefaultAsync(d => d.Id == notification.Id);
            }
            //defectionSpot.X = notification.X;
            //defectionSpot.Y = notification.Y;
            //defectionSpot.Description = notification.Description;
            //defectionSpot.OrderNumber = notification.OrderNumber;

            //await NormalizeOrderNumbers(notification.ChecklistItemId);
            //await _rootContext.SaveChangesAsync();
            //notification.Id = defectionSpot.Id;
        }

        private async Task NormalizeOrderNumbers(int checklistItemId)
        {
            var defectSpots = (await _rootContext.ChecklistItems
                .Include(x => x.DefectionSpots)
                .SingleAsync(x => x.Id == checklistItemId)).DefectionSpots.OrderBy(x => x.OrderNumber).ToList();

            int index = 1;
            foreach (var defectSpot in defectSpots)
            {
                defectSpot.OrderNumber = index++;
            }
        }
    }
}
