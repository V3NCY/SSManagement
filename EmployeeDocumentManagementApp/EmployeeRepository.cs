using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EmployeeDocumentManagementApp
{
    public class EmployeeRepository
    {
        private static AppDbContext context = new AppDbContext();
        private static Random random = new Random();
        
        public static ObservableCollection<Employee> GetEmployeesList()
        {
            var employeesList = context.Employees.ToList();
            return new ObservableCollection<Employee>(employeesList);
        }

        public static void AddEmployee(Employee employee)
        {
            employee.EmployeeId = GenerateUniqueId();
            context.Employees.Add(employee);
            context.SaveChanges();
        }

        //public static void RemoveEmployee(Employee employee)
        //{
        //    context.Employees.Remove(employee);
        //    context.SaveChanges();
        //}

        public static Employee GetEmployeeByName(string name)
        {
            return context.Employees.FirstOrDefault(e => e.EmployeeName == name);
        }

        private static int GenerateUniqueId()
        {
            return random.Next(100, 1000);
        }

        public static void UpdateEmployee(Employee employee)
        {
            var existingEmployee = context.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
            if (existingEmployee != null)
            {
                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.EGN = employee.EGN;
                existingEmployee.EmployeeName = employee.EmployeeName;
                existingEmployee.RemainingLeaveDays = employee.RemainingLeaveDays;
                existingEmployee.JobTitle = employee.JobTitle;
                existingEmployee.Department = employee.Department;

                context.SaveChanges();
            }
        }
    }
}
