using Application.Abstracts.Data;
using Domain.Sales;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Sales.Queries.GetSaleDetail
{
    public class GetSaleDetailQuery : IGetSaleDetailQuery
    {
        private readonly IRepository<Sale> _saleRepository;

        public GetSaleDetailQuery(IRepository<Sale> saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<SaleDetailModel> ExecuteAsync(int saleId)
        {
            var sale = _saleRepository.Get()
                .Where(p => p.Id == saleId)
                .Select(p => new SaleDetailModel()
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

            return await _saleRepository.SingleOrDefaultAsync(sale);
        }
    }
}
