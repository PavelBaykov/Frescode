using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ICSharpCode.SharpZipLib.Zip;
using unirest_net.http;

namespace Frescode.Controllers
{
    public class PdfConverterController : Controller
    {
        [HttpPost]
        public ActionResult ConvertPdfToImage(int resultWidth)
        {
            var file = Request.Files[0];

            var response = Unirest.post("https://do.convertapi.com/Pdf2Image?ApiKey=302872137&OutputFormat=jpg")
                .field("ToConvert.pdf", file.InputStream, "file")
                .asBinary();
            var memoryStream = new MemoryStream();
            response.Raw.CopyTo(memoryStream);
            memoryStream.Flush();

            var zf = new ZipFile(memoryStream);
            var images = new List<Stream>();
            foreach (ZipEntry zipEntry in zf)
            {
                var stream = zf.GetInputStream(zipEntry);
                images.Add(stream);
            }
            var resultStream = CombineImages(images, resultWidth);
            var resultData = resultStream.ToArray();
            return File(resultData, "image/png");
        }

        public static MemoryStream CombineImages(List<Stream> imagesData, int resultWidth)
        {
            var images = imagesData
                .Select(Image.FromStream)
                .Select(image => new Bitmap(image, new Size(resultWidth, image.Height * resultWidth / image.Width)))
                .ToList();

            var maxWidth = images.Max(x => x.Width);
            var totalHeight = images.Sum(x => x.Height);

            var bitmap = new Bitmap(maxWidth, totalHeight, PixelFormat.Format32bppPArgb);
            var g = Graphics.FromImage(bitmap);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            var currentY = 0;
            foreach (var image in images)
            {
                g.DrawImage(image, (maxWidth - image.Width) / 2, currentY);
                currentY += image.Height;
            }

            var ms = new MemoryStream(bitmap.Width * bitmap.Height);
            bitmap.Save(ms, ImageFormat.Png);
            ms.Flush();
            return ms;
        }
    }


}