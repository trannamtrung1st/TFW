using Application.Abstracts.Data;
using Domain.Customers;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Customers.Queries.GetCustomersList
{
    public class GetCustomersListQuery : IGetCustomersListQuery
    {
        private readonly IRepository<Customer> _customerRepository;

        public GetCustomersListQuery(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerModel[]> ExecuteAsync()
        {
            var query = _customerRepository.Get()
                .Select(p => new CustomerModel()
                {
                    Id = p.Id,
                    Name = p.Name
                });

            return await _customerRepository.ToArrayAsync(query);
        }
    }
}
