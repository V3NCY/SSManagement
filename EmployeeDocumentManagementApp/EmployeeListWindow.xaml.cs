using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace EmployeeDocumentManagementApp
{
    public partial class EmployeeListWindow : Window
    {
        private static readonly AppDbContext context = new AppDbContext();
        public ICommand RefreshCommand => new RelayCommand(LoadEmployeeList);

        private ObservableCollection<Employee> employeesList;

        public EmployeeListWindow()
        {
            InitializeComponent();
            DataContext = this;
            employeesList = EmployeeRepository.GetEmployeesList();
            lvEmployees.ItemsSource = employeesList;
            SubscribeToEmployeeChanges();
        }

        public void LoadEmployeeList()
        {
            var newEmployeeList = EmployeeRepository.GetEmployeesList();

            employeesList.Clear();
            foreach (var employee in newEmployeeList)
            {
                employeesList.Add(employee);
            }

            lvEmployees.ItemsSource = null;
            lvEmployees.ItemsSource = employeesList;
        }

        private void EmployeesList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        private void SubscribeToEmployeeChanges()
        {
            if (employeesList != null)
            {
                employeesList.CollectionChanged += EmployeesList_CollectionChanged;
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

        private class RelayCommand : ICommand
        {
            private readonly Action _execute;

            public RelayCommand(Action execute)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
                _execute();
            }

            public static ObservableCollection<Employee> RowVersion()
            {
                using (var context = new AppDbContext())
                {
                    return new ObservableCollection<Employee>(
                        context.Employees
                               .Include(e => e.PaidLeave)
                               .Include(e => e.UnpaidLeave)
                               .Include(e => e.OtherLeave)
                               .ToList()
                    );
                }
            }
        }

    }
}