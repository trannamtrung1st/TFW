using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Docs.Cross.Entities
{
    public class PostLocalization : AppLocalizationEntity<int, Post>
    {
        public string Index { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
