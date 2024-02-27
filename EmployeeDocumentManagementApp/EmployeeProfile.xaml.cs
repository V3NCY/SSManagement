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

    }
}
