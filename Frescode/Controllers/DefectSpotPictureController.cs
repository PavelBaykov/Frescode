﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Frescode.DAL;
using Frescode.DAL.Entities;

namespace Frescode.Controllers
{
    public class DefectSpotPictureController : Controller
    {
        private readonly RootContext _rootContext;

        public DefectSpotPictureController(RootContext rootContext)
        {
            _rootContext = rootContext;
        }

        [HttpPost]
        public ActionResult UploadFiles(int defectSpotId)
        {
            var statuses = new List<FilesStatus>();

            foreach (string file in Request.Files)
            {
                var headers = Request.Headers;

                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                {
                    UploadWholeFile(defectSpotId, Request, statuses);
                }
                else
                {
                    UploadPartialFile(defectSpotId, headers["X-File-Name"], Request, statuses);
                }

                break;
            }

            return Json(new { files = statuses });
        }


        [HttpGet]
        public ActionResult GetFiles(int defectSpotId)
        {
            var defectSpot = _rootContext.DefectionSpots
                .Include(x => x.AttachedPictures)
                .Single(x => x.Id == defectSpotId);

            return Json(
                new
                {
                    files =
                        defectSpot.AttachedPictures.Select(x => new FilesStatus(x.Name, x.PictureId)).ToArray()
                }, JsonRequestBehavior.AllowGet);
        }

        private void UploadWholeFile(int defectSpotId, HttpRequestBase request, List<FilesStatus> statuses)
        {
            var defectSpot = _rootContext.DefectionSpots
                .Include(x => x.AttachedPictures)
                .Single(x => x.Id == defectSpotId);

            var addedPictures = new List<Picture>();
            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];

                var memoryStream = new MemoryStream();
                file.InputStream.CopyTo(memoryStream);

                var pictureData = new PictureData
                {
                    Data = memoryStream.ToArray()
                };
                var picture = new Picture
                {
                    DateCaptured = DateTime.UtcNow,
                    PictureData = pictureData,
                    Name = file.FileName
                };

                defectSpot.AttachedPictures.Add(picture);
                addedPictures.Add(picture);
            }
            _rootContext.SaveChanges();
            statuses.AddRange(addedPictures.Select(picture => new FilesStatus(picture.Name, picture.PictureId)));
        }

        private void UploadPartialFile(int defectSpotId, string fileName, HttpRequestBase request, List<FilesStatus> statuses)
        {
            if (request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var file = request.Files[0];

            var memoryStream = new MemoryStream();
            file.InputStream.CopyTo(memoryStream);

            var pictureData = new PictureData
            {
                Data = memoryStream.ToArray()
            };
            var picture = new Picture
            {
                DateCaptured = DateTime.UtcNow,
                PictureData = pictureData,
                Name = fileName
            };

            var defectSpot = _rootContext.DefectionSpots.Single(x => x.Id == defectSpotId);
            defectSpot.AttachedPictures.Add(picture);
            _rootContext.SaveChanges();

            statuses.Add(new FilesStatus(file.FileName, picture.PictureId));
        }

    }

    public class FilesStatus
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public string DeleteUrl { get; set; }
        public string DeleteType { get; set; }

        public FilesStatus(string fileName, int pictureId)
        {
            Name = fileName;
            Type = "image/png";
            Url = $"/Picture/GetPicture?pictureId={pictureId}";
            DeleteUrl = $"/Picture/DeletePicture?pictureId={pictureId}";
            DeleteType = "GET";
            ThumbnailUrl = $"/Picture/GetPictureThumbnail?pictureId={pictureId}";
        }
    }
}