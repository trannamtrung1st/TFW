using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.CQRSExamples.Models.Query
{
    public interface IProductCategoryQuery
    {
        Task<IEnumerable<ProductCategoryListItem>> GetProductCategoryListAsync();
        Task<IEnumerable<ProductCategoryListOption>> GetProductCategoryListOptionAsync();
        Task<ProductCategoryDetail> GetProductCategoryDetailAsync(string id);
    }

    public class ProductCategoryDetail
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ProductCategoryListItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductCount { get; set; }
    }

    public class ProductCategoryListOption
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
