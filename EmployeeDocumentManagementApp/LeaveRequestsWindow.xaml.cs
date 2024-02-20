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

            int leaveDays = GetLeaveDaysToDeduct();

            if (IsPaidLeave())
            {
                if (employee.RemainingLeaveDays < leaveDays)
                {
                    MessageBox.Show("Служителят няма достатъчно оставащи отпускни дни за избрания период.");
                    return;
                }

                employee.RemainingLeaveDays -= leaveDays;
                EmployeeRepository.UpdateEmployee(employee);
                EmployeeRepository.UpdatePaidLeaveRecords(employee.EmployeeId, DateTime.Today);
            }

            MessageBox.Show("Отпуската е записана успешно!");
            Close();
        }

        private bool IsPaidLeave()
        {
            return chkPaidLeave.IsChecked ?? false;
        }

        private int GetLeaveDaysToDeduct()
        {
            if (IsPaidLeave() && textBoxLeaveDays != null) 
            {
                int leaveDays;
                if (int.TryParse(textBoxLeaveDays.Text, out leaveDays))
                {
                    return leaveDays;
                }
            }
            return 0;
        }
    }
}
