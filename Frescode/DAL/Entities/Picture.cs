using System;

namespace Frescode.DAL.Entities
{
    public class Picture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCaptured { get; set; }
        public DefectionSpot DefectionSpot { get; set; }
        public PictureData PictureData { get; set; }
        public int PictureId { get; set; }
    }

    public class PictureData
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }
    }
}