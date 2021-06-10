using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Customers.Queries.GetCustomersList
{
    public interface IGetCustomersListQuery
    {
        Task<CustomerModel[]> ExecuteAsync();
    }
}
