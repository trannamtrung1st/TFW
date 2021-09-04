using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFW.Framework.CQRSExamples.Models.Query
{
    public interface IStoreQuery
    {
        Task<IEnumerable<StoreListItem>> GetStoreListAsync();
        Task<IEnumerable<StoreListOption>> GetStoreListOptionAsync();
    }

    public class StoreListItem
    {
        public string Id { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
    }

    public class StoreListOption
    {
        public string Id { get; set; }
        public string StoreName { get; set; }
    }
}
