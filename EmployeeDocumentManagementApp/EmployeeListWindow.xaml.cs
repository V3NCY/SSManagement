using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EmployeeDocumentManagementApp;

namespace EmployeeDocumentManagementApp
{
    public partial class EmployeeListWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Employee> employeesList;

        public ICommand RefreshCommand => new RelayCommand(() => LoadEmployeeList());


        public EmployeeListWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadEmployeeList();
            SubscribeToEmployeeChanges();
        }

        public void LoadEmployeeList()
        {
            try
            {
                var newEmployeeList = EmployeeRepository.GetEmployeesList();
                var activeEmployees = newEmployeeList.Where(emp => !emp.IsArchived);
                EmployeesList = new ObservableCollection<Employee>(activeEmployees);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem loading the employees list: {ex.Message}");
            }
        }


        public ObservableCollection<Employee> EmployeesList
        {
            get { return employeesList; }
            set
            {
                employeesList = value;
                OnPropertyChanged(nameof(EmployeesList));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SubscribeToEmployeeChanges()
        {
            if (employeesList != null)
            {
                employeesList.CollectionChanged += EmployeesList_CollectionChanged;
            }
        }

        private void EmployeesList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }

        private void OnRefreshButtonClick(object sender, RoutedEventArgs e)
        {
            LoadEmployeeList();
        }

        private void OnDeleteMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (lvEmployees.SelectedItem is Employee selectedEmployee && selectedEmployee != null)
            {
                MessageBoxResult result = MessageBox.Show("Сигурни ли сте, че искате да архивирате този служител?", "Потвърждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        ArchiveEmployeeRepository.ArchiveEmployee(selectedEmployee);
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
                                    ArchiveEmployeeRepository.ArchiveEmployee(conflictingEmployee);
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
        }

        private void OnArchiveMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (lvEmployees.SelectedItem is Employee selectedEmployee && selectedEmployee != null)
            {
                MessageBoxResult result = MessageBox.Show("Сигурни ли сте, че искате да архивирате този служител?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        ArchiveEmployeeRepository.ArchiveEmployee(selectedEmployee);
                        EmployeesList.Remove(selectedEmployee); 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error archiving employee: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Employee selectedEmployee = (Employee)lvEmployees.SelectedItem;
            if (selectedEmployee != null)
            {
                EditEmployeeWindow editWindow = new EditEmployeeWindow(this, selectedEmployee);
                editWindow.ShowDialog();
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
        }
    }
}
