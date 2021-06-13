using Application.Abstracts.Data;
using Domain.Sales;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Sales.Queries.GetSalesList
{
    public class GetSalesListQuery : IGetSalesListQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Sale> _saleRepository;

        public GetSalesListQuery(IUnitOfWork uow,
            IRepository<Sale> saleRepository)
        {
            _uow = uow;
            _saleRepository = saleRepository;
        }

        public async Task<SalesListItemModel[]> ExecuteAsync()
        {
            var query = _saleRepository.IgnoreQueryFilters()
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

            return await _uow.ToArrayAsync(query);
        }
    }
}
