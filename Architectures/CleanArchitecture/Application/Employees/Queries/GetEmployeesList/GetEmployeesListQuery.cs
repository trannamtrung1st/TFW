using Application.Abstracts.Data;
using Domain.Employees;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Employees.Queries.GetEmployeesList
{
    public class GetEmployeesListQuery : IGetEmployeesListQuery
    {
        private readonly IRepository<Employee> _employeeRepository;

        public GetEmployeesListQuery(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeModel[]> ExecuteAsync()
        {
            var query = _employeeRepository.Get()
                .Select(p => new EmployeeModel
                {
                    Id = p.Id,
                    Name = p.Name
                });

            return await _employeeRepository.ToArrayAsync(query);
        }
    }
}
