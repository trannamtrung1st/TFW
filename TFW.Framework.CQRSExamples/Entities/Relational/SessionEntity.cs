using System;

namespace TFW.Framework.CQRSExamples.Entities.Relational
{
    public class SessionEntity
    {
        public string Id { get; set; }
        public DateTimeOffset InitTime { get; set; }
    }
}
