using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Frescode.DAL;
using Frescode.DAL.Entities;
using MediatR;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;
using System;
using PdfSharp.Drawing.Layout;

namespace Frescode.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        public ReportController(IMediator mediator, RootContext rootContext)
            :base(mediator, rootContext)
        {
        }

        [HttpGet]
        public ActionResult GenerateReport(int projectId)
        {
            var project = Context.Projects
                .Include(x => x.Members)
                .Include(x => x.Checklists)
                .Include(x => x.Checklists.Select(c => c.ChecklistTemplate))
                .Include(x => x.Checklists.Select(c => c.Items))
                .Include(x => x.Checklists.Select(c => c.Items.Select(i => i.DefectionSpots)))
                .Include(x => x.Checklists.Select(c => c.Items.Select(i => i.ItemTemplate)))
                .Include(x => x.Checklists.Select(c => c.Items.Select(i => i.DefectionSpots)))
                .Single(x => x.Id == projectId);


            return File(GenerateReportData(project), "application/pdf");
        }

        private XGraphics _currentGraphics;
        private XTextFormatter _formatter;
        private PdfPage _currentPage;
        private PdfDocument _document;
        private double _currentTop;

        private byte[] GenerateReportData(Project project)
        {
            _document = new PdfDocument();

            _currentPage = _document.AddPage();

            _currentGraphics = XGraphics.FromPdfPage(_currentPage);
            _formatter = new XTextFormatter(_currentGraphics);

            _formatter.DrawString("COMPANY LOGO", new XFont("Arial", 18, XFontStyle.Bold), XBrushes.Black,
                new XRect(50, 35, _currentPage.Width, 50),
                XStringFormats.TopLeft);

            _formatter.DrawString(DateTime.Now.ToShortDateString(), new XFont("Arial", 13, XFontStyle.Regular), XBrushes.Black,
                new XRect(460, 40, _currentPage.Width, 50),
                XStringFormats.TopLeft);


            _formatter.DrawString(project.Name.ToUpper(), new XFont("Arial", 13, XFontStyle.Bold), XBrushes.Black,
                new XRect(50, 90, _currentPage.Width, 50),
                XStringFormats.TopLeft);

            _formatter.DrawString("Date started:", new XFont("Arial", 13, XFontStyle.Regular), XBrushes.Black,
                new XRect(50, 120, _currentPage.Width, 50),
                XStringFormats.TopLeft);

            _formatter.DrawString(project.DateCreated.ToShortDateString(), new XFont("Arial", 13, XFontStyle.Regular), XBrushes.Black,
                new XRect(180, 120, _currentPage.Width, 50),
                XStringFormats.TopLeft);


            _formatter.DrawString("Members list:", new XFont("Arial", 13, XFontStyle.Regular), XBrushes.Black,
                new XRect(50, 150, _currentPage.Width, 50),
                XStringFormats.TopLeft);

            var members = project.Members.OrderBy(m => m.FirstName).ToList();
            var halfCount = members.Count;
            for (var i = 0; i < members.Count; ++i)
            {
                _formatter.DrawString($"{members[i].FirstName} {members[i].LastName}", new XFont("Arial", 13, XFontStyle.Regular), XBrushes.Black,
                    new XRect(180 + 80 * (i / halfCount), 150 + (i % halfCount) * 20, _currentPage.Width, 50),
                    XStringFormats.TopLeft);
            }

            _currentTop = 200;
            foreach (var checklist in project.Checklists)
            {
                var checklistName = checklist.ChecklistTemplate.Name;
                var height = MeasureHeight(_currentGraphics, checklistName, new XFont("Arial", 13, XFontStyle.Regular), (int)_currentPage.Width + 160);
                _formatter.DrawString(checklistName, new XFont("Arial", 13, XFontStyle.Regular), XBrushes.Black,
                    new XRect(50, _currentTop, _currentPage.Width - 50, height),
                    XStringFormats.TopLeft);

                _currentTop += height + 10;

                foreach (var item in checklist.Items)
                {
                    var itemName = item.ItemTemplate.Name;
                    var itemDescription = item.ItemTemplate.Description;
                    var itemHeight = MeasureHeight(_currentGraphics, itemName, new XFont("Arial", 13, XFontStyle.Regular), (int)_currentPage.Width + 160);

                    _formatter.DrawString("• " + itemName, new XFont("Arial", 13, XFontStyle.Regular), XBrushes.Black,
                        new XRect(80, _currentTop, _currentPage.Width - 50 - 80, _currentPage.Height),
                        XStringFormats.TopLeft);
                    _currentTop += itemHeight + 5;
                    var itemDescriptionHeight = MeasureHeight(_currentGraphics, itemDescription, new XFont("Arial", 13, XFontStyle.Regular), (int)_currentPage.Width + 160);
                    CreateNewPageIfNeed(itemDescriptionHeight + 5);
                    _formatter.DrawString(itemDescription, new XFont("Arial", 13, XFontStyle.Regular), XBrushes.Black,
                        new XRect(88, _currentTop, _currentPage.Width - 50 - 80, itemDescriptionHeight),
                        XStringFormats.TopLeft);
                    _currentTop += itemDescriptionHeight + 5;

                    foreach (var spot in item.DefectionSpots)
                    {
                        var description = spot.Description;
                        var spotDescriptionHeight = MeasureHeight(_currentGraphics, description, new XFont("Arial", 13, XFontStyle.Regular), (int)_currentPage.Width + 160);
                        CreateNewPageIfNeed(spotDescriptionHeight + 5);
                        _formatter.DrawString("-", new XFont("Arial", 12, XFontStyle.Regular), XBrushes.Black,
                            new XRect(93, _currentTop, _currentPage.Width - 50 - 93, spotDescriptionHeight),
                            XStringFormats.TopLeft);
                        _formatter.DrawString(description, new XFont("Arial", 12, XFontStyle.Regular), XBrushes.Black,
                            new XRect(102, _currentTop, _currentPage.Width - 50 - 102, spotDescriptionHeight),
                            XStringFormats.TopLeft);

                        _currentTop += spotDescriptionHeight + 5;
                    }
                }

                CreateNewPageIfNeed(20);
                _currentTop += 20;
            }

            var memoryStream = new MemoryStream();
            _document.Save(memoryStream);
            return memoryStream.ToArray();
        }

        private void CreateNewPageIfNeed(double heightToAdd)
        {
            if (_currentTop + heightToAdd > _currentPage.Height - 50)
            {
                _currentPage = _document.AddPage();
                _currentGraphics = XGraphics.FromPdfPage(_currentPage);
                _formatter = new XTextFormatter(_currentGraphics);
                _currentTop = 50;
            }
        }

        public double MeasureHeight(PdfSharp.Drawing.XGraphics gfx, string text, PdfSharp.Drawing.XFont font, int width)
        {
            var lines = text.Split('\n');

            double totalHeight = 0;

            foreach (string line in lines)
            {
                var size = gfx.MeasureString(line, font);
                double height = size.Height + (size.Height * Math.Floor(size.Width / width));

                totalHeight += height;
            }

            return totalHeight;
        }
    }
}