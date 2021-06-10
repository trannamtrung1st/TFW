using Application.Abstracts.Data;
using Domain.Sales;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Sales.Queries.GetSalesList
{
    public class GetSalesListQuery : IGetSalesListQuery
    {
        private readonly IRepository<Sale> _saleRepository;

        public GetSalesListQuery(IRepository<Sale> saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<SalesListItemModel[]> ExecuteAsync()
        {
            var query = _saleRepository.Get()
                .Select(p => new SalesListItemModel()
                {
                    Id = p.Id,
                    Date = p.Date,
                    CustomerName = p.Customer.Name,
                    EmployeeName = p.Employee.Name,
                    ProductName = p.Product.Name,
                    UnitPrice = p.UnitPrice,
                    Quantity = p.Quantity,
                    TotalPrice = p.TotalPrice
                });

            return await _saleRepository.ToArrayAsync(query);
        }
    }
}
