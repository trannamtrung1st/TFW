using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Mime;
using System.Text;

namespace TFW.Framework.Http.Contents
{
    public class JsonContent : StringContent
    {
        public const string JsonMediaType = MediaTypeNames.Application.Json;

        public JsonContent(object content) : base(JsonConvert.SerializeObject(content), Encoding.UTF8, JsonMediaType)
        {
        }

        public JsonContent(object content, Encoding encoding) : base(JsonConvert.SerializeObject(content), encoding, JsonMediaType)
        {
        }

        public JsonContent(object content, Encoding encoding, string mediaType) : base(JsonConvert.SerializeObject(content), encoding, mediaType)
        {
        }
    }
}
