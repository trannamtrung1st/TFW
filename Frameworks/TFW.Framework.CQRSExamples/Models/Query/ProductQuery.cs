using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFW.Framework.CQRSExamples.Models.Query
{
    public interface IProductQuery
    {
        Task<IEnumerable<ProductListItem>> GetProductListAsync();
        Task<ProductDetail> GetProductDetailAsync(string id);
    }

    public class ProductDetail
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string StoreId { get; set; }
        public string StoreName { get; set; }
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public double UnitPrice { get; set; }
    }

    public class ProductListItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string StoreName { get; set; }
        public string BrandName { get; set; }
        public double UnitPrice { get; set; }
    }
}
