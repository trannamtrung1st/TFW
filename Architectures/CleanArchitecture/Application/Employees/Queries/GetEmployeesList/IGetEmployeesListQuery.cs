using System.Threading.Tasks;

namespace Application.Employees.Queries.GetEmployeesList
{
    public interface IGetEmployeesListQuery
    {
        Task<EmployeeModel[]> ExecuteAsync();
    }
}
