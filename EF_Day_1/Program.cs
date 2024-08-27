using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Day_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new Company_SDEntities())
            {
                // Select all data from Department
                var departments = context.Departments.ToList();
                Console.WriteLine("All Departments:");
                foreach (var dept in departments)
                {
                    Console.WriteLine($"Dnum: {dept.Dnum}, Dname: {dept.Dname}, MGRSSN: {dept.MGRSSN}, MGRStart_Date: {dept.MGRStart_Date}");
                }

                // Select all data from Employee
                var employees = context.Employees.ToList();
                Console.WriteLine("\nAll Employees:");
                foreach (var emp in employees)
                {
                    Console.WriteLine($"SSN: {emp.SSN}, Name: {emp.Fname} {emp.Lname}, Address: {emp.Address}, Salary: {emp.Salary}");
                }

                // Select department where Dnum = 10 using IQueryable
                var deptQuery = context.Departments.Where(d => d.Dnum == 10);
                Console.WriteLine("\nDepartment with Dnum = 10 (IQueryable):");
                foreach (var dept in deptQuery)
                {
                    Console.WriteLine($"Dnum: {dept.Dnum}, Dname: {dept.Dname}, MGRSSN: {dept.MGRSSN}, MGRStart_Date: {dept.MGRStart_Date}");
                }

                // Select department where Dnum = 10 using IEnumerable
                var deptEnum = context.Departments.ToList().Where(d => d.Dnum == 10);
                Console.WriteLine("\nDepartment with Dnum = 10 (IEnumerable):");
                foreach (var dept in deptEnum)
                {
                    Console.WriteLine($"Dnum: {dept.Dnum}, Dname: {dept.Dname}, MGRSSN: {dept.MGRSSN}, MGRStart_Date: {dept.MGRStart_Date}");
                }
            }

            using (var context = new Company_SDEntities())
            {
                // Join Department and Employee tables and select department name and employee name
                var query = from dept in context.Departments
                            join emp in context.Employees on dept.Dnum equals emp.Dno
                            select new { DepartmentName = dept.Dname, EmployeeName = emp.Fname + " " + emp.Lname };

                Console.WriteLine("Department Name and Employee Name:");
                foreach (var item in query)
                {
                    Console.WriteLine($"Department: {item.DepartmentName}, Employee: {item.EmployeeName}");
                }
            }

            using (var context = new Company_SDEntities())
            {
                // Using LINQ Select method to select Fname and Lname from Employee as FirstName, LastName
                var employeeNames = context.Employees
                                           .Select(e => new { FirstName = e.Fname, LastName = e.Lname })
                                           .ToList();

                Console.WriteLine("Employee Names:");
                foreach (var emp in employeeNames)
                {
                    Console.WriteLine($"FirstName: {emp.FirstName}, LastName: {emp.LastName}");
                }

                // Using LINQ SelectMany method to select DepartmentName and related Employee FName and Salary
                var departmentEmployees = context.Departments
                                                 .SelectMany(d => d.Employees, (d, e) => new
                                                 {
                                                     DepartmentName = d.Dname,
                                                     EmployeeName = e.Fname,
                                                     Salary = e.Salary ?? 50000 // Default value for null salary
                                                 })
                                                 .ToList();

                Console.WriteLine("\nDepartment and Employee Details:");
                foreach (var item in departmentEmployees)
                {
                    Console.WriteLine($"Department: {item.DepartmentName}, Employee: {item.EmployeeName}, Salary: {item.Salary}");
                }
            }

        }
    }
}
