using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EmployeeDocumentManagementApp
{
    public partial class EmployeeProfile : Window
    {
        private static List<EmployeeProfile> openWindows = new List<EmployeeProfile>();
        public EmployeeProfile(Employee employee)
        {
            InitializeComponent();
            DataContext = employee;
            openWindows.Add(this);
            Closed += EmployeeProfile_Closed;
        }
        private void EmployeeProfile_Closed(object sender, EventArgs e)
        {
            openWindows.Remove(this);
        }

        public static bool IsWindowOpen()
        {
            return openWindows.Any();
        }

        public static void BringToFront()
        {
            if (openWindows.Any())
            {
                openWindows.Last().Activate();
            }
        }
        private void OpenInformation_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string tag = button.Tag as string;
                switch (tag)
                {
                    case "BankInformation":
                        BankInformationWindow bankInfoWindow = new BankInformationWindow();
                        bankInfoWindow.Show();
                        break;
                    case "Documents":
                        DocumentsWindow documentsWindow = new DocumentsWindow();
                        documentsWindow.Show();
                        break;
                    case "Salary":
                        SalaryWindow salaryWindow = new SalaryWindow();
                        salaryWindow.Show();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
