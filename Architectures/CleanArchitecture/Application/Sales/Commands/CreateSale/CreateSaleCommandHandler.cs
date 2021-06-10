using Application.Abstracts.Data;
using Application.Abstracts.Services;
using Application.Sales.Commands.CreateSale.Factories;
using Cross.Dates;
using Domain.Customers;
using Domain.Employees;
using Domain.Products;
using Domain.Sales;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Sales.Commands.CreateSale
{
    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, Sale>
    {
        private readonly IDateService _dateService;
        private readonly IDbContext _dbContext;
        private readonly ISaleFactory _saleFactory;
        private readonly IRepository<Sale> _saleRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IInventoryService _inventoryService;

        public CreateSaleCommandHandler(
            IDateService dateService,
            IDbContext dbContext,
            ISaleFactory saleFactory,
            IRepository<Sale> saleRepository,
            IRepository<Customer> customerRepository,
            IRepository<Employee> employeeRepository,
            IRepository<Product> productRepository,
            IInventoryService inventory)
        {
            _dateService = dateService;
            _dbContext = dbContext;
            _saleFactory = saleFactory;
            _saleRepository = saleRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _productRepository = productRepository;
            _inventoryService = inventory;
        }

        public async Task<Sale> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var date = _dateService.GetDate();

            var customer = _customerRepository.Get()
                .Single(p => p.Id == request.CustomerId);

            var employee = _employeeRepository.Get()
                .Single(p => p.Id == request.EmployeeId);

            var product = _productRepository.Get()
                .Single(p => p.Id == request.ProductId);

            var quantity = request.Quantity;

            var sale = _saleFactory.Create(date, customer, employee, product, quantity);

            _saleRepository.Add(sale);

            await _dbContext.SaveChangesAsync();

            _inventoryService.NotifySaleOcurred(product.Id, quantity);

            return sale;
        }
    }
}
