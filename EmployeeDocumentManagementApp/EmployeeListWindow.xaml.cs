using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EmployeeDocumentManagementApp;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;
using System.Collections.Generic;

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

            Loaded += EmployeeListWindow_Loaded;
        }
        private void MarkLeaveOnCalendar(Employee employee, DateTime selectedDate, DateTime leaveEndDate)
        {
            if (employee != null)
            {
                if (employee.PaidLeaveDates.Contains(selectedDate) && employee.PaidLeave && employee.GetTotalPaidLeaveDays() < 20)
                {
                    calendar.SelectedDates.Add(selectedDate);
                }

            }
        }
        private void AddConsecutiveLeaveDaysToCalendar(DateTime startDate, int consecutiveDays)
        {
            for (int i = 0; i < consecutiveDays; i++)
            {
                calendar.SelectedDates.Add(startDate.AddDays(i));
            }
        }

        private int GetConsecutiveLeaveDays(List<DateTime> leaveDates, DateTime leaveStartDate, DateTime leaveEndDate)
        {
            int count = 0;
            for (DateTime date = leaveStartDate; date <= leaveEndDate; date = date.AddDays(1))
            {
                if (leaveDates.Contains(date))
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }


        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Employee employee = (Employee)lvEmployees.SelectedItem;
            if (employee != null)
            {
                foreach (DateTime selectedDate in calendar.SelectedDates)
                {
                    DateTime leaveEndDate = selectedDate;
                    int consecutiveLeaveDays = GetConsecutiveLeaveDays(employee.PaidLeaveDates, selectedDate, leaveEndDate);

                    if (consecutiveLeaveDays > 0)
                    {
                        AddConsecutiveLeaveDaysToCalendar(selectedDate, consecutiveLeaveDays);
                    }
                }
            }
        }

        private void OnLeaveRequestEntered(Employee employee, DateTime leaveDate)
        {
            if (employee != null)
            {
                if (employee.PaidLeave)
                {
                    employee.PaidLeaveDates.Add(leaveDate);

                    int totalLeaveDaysTaken = employee.GetTotalPaidLeaveDays();

                    int remainingPaidLeaveDays = 20 - totalLeaveDaysTaken;

                    if (remainingPaidLeaveDays >= 0)
                    {
                        DateTime leaveEndDate = leaveDate.AddDays(1);
                        MarkLeaveOnCalendar(employee, leaveDate, leaveEndDate);
                    }
                    else
                    {
                        MessageBox.Show("No remaining paid leave days.");
                    }
                }
                else if (employee.UnpaidLeave)
                {
                    employee.UnpaidLeaveDates.Add(leaveDate);
                }
                else if (employee.OtherLeave)
                {
                    employee.OtherLeaveDates.Add(leaveDate);
                }

                RefreshEmployeeList();
            }
        }


        private async void EmployeeListWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadEmployeeListAsync();
        }
        public async Task LoadEmployeeListAsync()
        {
            try
            {
                var newEmployeeList = await Task.Run(() => EmployeeRepository.GetEmployeesList());
                var activeEmployees = newEmployeeList.Where(emp => !emp.IsArchived);
                EmployeesList = new ObservableCollection<Employee>(activeEmployees);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Грешка при зареждане на списъка със служители: {ex.Message}");
            }
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
                Console.WriteLine($"Проблем при зареждането на списъка със служители: {ex.Message}");
            }
        }
        public void RefreshEmployeeList()
        {
            try
            {
                var newEmployeeList = EmployeeRepository.GetEmployeesList();
                var activeEmployees = newEmployeeList.Where(emp => !emp.IsArchived);
                EmployeesList = new ObservableCollection<Employee>(activeEmployees);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading employee list: {ex.Message}");
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
                                    MessageBox.Show("Този служител е бил архивиран от друг изпълнител.");
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
                        MessageBox.Show($"Грешка при архивиране на служителя: {ex.Message}", "Грешка!", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void lvEmployees_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}