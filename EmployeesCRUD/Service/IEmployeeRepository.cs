using EmployeesCRUD.Model;

namespace EmployeesCRUD.Service
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployees();

        Task<Employee>Insert(Employee employee);

        Task<Employee> GetEmployeesById(string empid);

        Task<int> DeleteByEmployeeId(string empid);

        Task<int> UpdateEmployee(Employee employee);

        Task<int> Login(Login login);


    }
}
