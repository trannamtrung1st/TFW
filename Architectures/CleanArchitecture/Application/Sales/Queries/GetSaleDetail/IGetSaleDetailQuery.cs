using System.Threading.Tasks;

namespace Application.Sales.Queries.GetSaleDetail
{
    public interface IGetSaleDetailQuery
    {
        Task<SaleDetailModel> ExecuteAsync(int id);
    }
}
