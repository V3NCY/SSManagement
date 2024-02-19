using System;
using System.Windows;

namespace EmployeeDocumentManagementApp
{
    public partial class LeaveRequestsWindow : Window
    {
        public LeaveRequestsWindow()
        {
            InitializeComponent();
        }

        private void OnSubmitButtonClick(object sender, RoutedEventArgs e)
        {
            string employeeIdentifier = txtEmployeeIdentifier.Text;
            Employee employee;

            // Check if the input is an ID or a name
            if (int.TryParse(employeeIdentifier, out int employeeId))
            {
                employee = EmployeeRepository.GetEmployeeById(employeeId);
            }
            else
            {
                employee = EmployeeRepository.GetEmployeeByName(employeeIdentifier);
            }

            if (employee == null)
            {
                MessageBox.Show("Служителят не е намерен. Моля първо го добавете към списъка.");
                return;
            }

            if (IsPaidLeave())
            {
                if (employee.RemainingLeaveDays <= 0)
                {
                    MessageBox.Show("Служителят е изчерпал своята отпуска за тази година.");
                    return;
                }

                employee.RemainingLeaveDays--;
            }

            MessageBox.Show("Отпуската е записана успешно!");
            Close();
        }

        private bool IsPaidLeave()
        {
            return chkPaidLeave.IsChecked ?? false;
        }
    }
}
