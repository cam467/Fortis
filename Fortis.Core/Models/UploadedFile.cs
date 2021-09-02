namespace Fortis.Core.Models
{
    using System.IO;
    public class UploadedFile
    {
        public string filename { get; set; }
        public Stream filestream { get; set; }
    }
}