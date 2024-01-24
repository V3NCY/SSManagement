using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class EmployeeListWindow : Window
    {
        private static readonly AppDbContext context = new AppDbContext();

        public EmployeeListWindow()
        {
            InitializeComponent();
            LoadEmployeeList();
            SubscribeToEmployeeChanges();
        }

        private void LoadEmployeeList()
        {
            lvEmployees.ItemsSource = EmployeeRepository.GetEmployeesList();
        }

        private void SubscribeToEmployeeChanges()
        {
            var employeesList = EmployeeRepository.GetEmployeesList();

            if (employeesList != null)
            {
                employeesList.CollectionChanged += (sender, e) => LoadEmployeeList();
            }
        }

        private void OnRefreshButtonClick(object sender, RoutedEventArgs e)
        {
            LoadEmployeeList();
        }

        private void MoveToArchive(Employee employee)
        {
            ArchiveEmployeeRepository.ArchiveEmployee(employee);
        }

        private void OnDeleteMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (lvEmployees.SelectedItem is Employee selectedEmployee && selectedEmployee != null)
            {
                try
                {
                    RemoveEmployee(selectedEmployee);
                    LoadEmployeeList();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Employee conflictingEmployee)
                        {
                            if (entry.OriginalValues["IsArchived"].Equals(true))
                            {
                                MessageBox.Show("This employee has already been archived by another user.");
                            }
                            else
                            {
                                entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                                RemoveEmployee(conflictingEmployee);
                                LoadEmployeeList();
                            }
                        }
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Console.WriteLine($"Entity: {entityValidationErrors.Entry.Entity.GetType().Name}, Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                        }
                    }
                }
            }
        }



        private void RemoveEmployee(Employee employee)
        {
            var entry = context.Entry(employee);

            if (entry.State == EntityState.Detached)
            {
                context.Employees.Attach(employee);
            }

            try
            {
                context.Entry(employee).State = EntityState.Deleted;
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var databaseValues = context.Entry(employee).GetDatabaseValues();

                if (databaseValues != null)
                {
                    context.Entry(employee).OriginalValues.SetValues(databaseValues);
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
