using System.Threading.Tasks;

namespace Application.Products.Queries.GetProductsList
{
    public interface IGetProductsListQuery
    {
        Task<ProductModel[]> ExecuteAsync();
    }
}
