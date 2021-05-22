using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.CQRSExamples.Models.Query
{
    public interface IBrandQuery
    {
        Task<IEnumerable<BrandListItem>> GetBrandListAsync();
        Task<IEnumerable<BrandListOption>> GetBrandListOptionAsync();
    }

    public class BrandListItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class BrandListOption
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
