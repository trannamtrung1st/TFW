using System.Threading.Tasks;

namespace Application.Sales.Queries.GetSalesList
{
    public interface IGetSalesListQuery
    {
        Task<SalesListItemModel[]> ExecuteAsync();
    }
}
