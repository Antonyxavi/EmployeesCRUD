using Dapper;
using EmployeesCRUD.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections;
using System.Collections.Generic;

namespace EmployeesCRUD.Service
{
    public class EmployeeRepository:IEmployeeRepository
    {
        private readonly DapperDb dapperDb;
        public EmployeeRepository(DapperDb dapperDb)
        {
            this.dapperDb = dapperDb;
        }

        public async Task<int> DeleteByEmployeeId(string empid)
        {
            try
            {
                string sql = string.Format("DELETE FROM Employee where EmployeeId = '{0}'",empid);
                using (var connection = dapperDb.CreateConnection())
                {
                    int employeeId = await connection.ExecuteAsync(sql);
                    return employeeId;
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            try
            {
                string sql = "SELECT * FROM Employee";
                using (var connection = dapperDb.CreateConnection())
                {
                    IEnumerable<Employee> employees = await connection.QueryAsync<Employee>(sql);
                    return employees;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Employee> GetEmployeesById(string empid)
        {
            try
            {
                string sql = string.Format("SELECT * FROM Employee where EmployeeId = '{0}'",empid);
                using (var connection = dapperDb.CreateConnection())
                {
                    Employee employees = await connection.QueryFirstOrDefaultAsync<Employee>(sql, new { empid });
                    return employees;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Employee> Insert(Employee employee)
        {
            try
            {
                string sql = @"INSERT INTO Employee(FirstName,LastName,EmployeeId,Age,PhoneNo,Address,Salary)
                          VALUES(@FirstName,@LastName,@EmployeeId,@Age,@PhoneNo,@Address,@Salary)";
                var parameters = new DynamicParameters();
                parameters.Add("id", employee.Id);
                parameters.Add("FirstName", employee.FirstName);
                parameters.Add("LastName", employee.LastName);
                parameters.Add("EmployeeId", employee.EmployeeId);
                parameters.Add("Age", employee.Age);
                parameters.Add("PhoneNo", employee.PhoneNo);
                parameters.Add("Address", employee.Address);
                parameters.Add("Salary", employee.Salary);
                using (var connection = dapperDb.CreateConnection())
                {
                    await connection.ExecuteAsync(sql, parameters);

                    Employee emp = new Employee()
                    {
                        EmployeeId = employee.EmployeeId,
                    };
                   return emp;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<int> Login(Login login)
        {
            string sql = string.Format("select count(*) from Login where username = '{0}' and password = '{1}'",login.UserName,login.Password);
            using (var connection = dapperDb.CreateConnection())
            {
                int count = await connection.QueryFirstOrDefaultAsync<int>(sql, new { login });


                return count;
            }

        }

        public async Task<int> UpdateEmployee(Employee employee)
        {
            try
            {
                string sql = string.Format("UPDATE employee SET    firstname ='{0}' ,lastname ='{1}', employeeid ='{2}', age ={3} ,phoneno ={4} ,address ='{5}' ,salary ={6} WHERE  employeeid ='{2}' ", employee.FirstName, employee.LastName, employee.EmployeeId, employee.Age, employee.PhoneNo, employee.Address, employee.Salary);
                using (var connection = dapperDb.CreateConnection())
                {
                    int empId = await connection.ExecuteAsync(sql, new { employee});


                    return empId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}
