using System;

namespace TFW.Docs.Cross.Models.PostCategory
{
    public class ListPostCategoryJoinModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Lang { get; set; }
        public string Region { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
