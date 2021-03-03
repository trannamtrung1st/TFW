using MimeKit;
using System.IO;

namespace TFW.Framework.SimpleMail.Models
{
    public class SimpleAttachment
    {
        public string FileName { get; set; }
        public Stream DataStream { get; set; }
        public ContentType ContentType { get; set; }

        private SimpleAttachment()
        {
        }

        public static SimpleAttachment From(string filePath)
        {
            var fileName = Path.GetFileName(filePath);

            return new SimpleAttachment
            {
                FileName = fileName,
                DataStream = new FileStream(filePath, FileMode.Open)
            };
        }

        public static SimpleAttachment From(string fileName, Stream dataStream, ContentType contentType = null)
        {
            return new SimpleAttachment
            {
                FileName = fileName,
                DataStream = dataStream,
                ContentType = contentType
            };
        }
    }
}
