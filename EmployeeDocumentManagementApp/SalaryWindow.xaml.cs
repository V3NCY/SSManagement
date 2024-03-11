using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EmployeeDocumentManagementApp
{

    public partial class SalaryWindow : Window
    {
        public SalaryWindow()
        {
            InitializeComponent();
        }
        public class SalaryMonth
        {
            public decimal January { get; set; }
            public decimal February { get; set; }
            public decimal March { get; set; }
            public decimal April { get; set; }
            public decimal May { get; set; }
            public decimal June { get; set; }
            public decimal July { get; set; }
            public decimal August { get; set; }
            public decimal September { get; set; }
            public decimal October { get; set; }
            public decimal November { get; set; }
            public decimal December { get; set; }
        }

        public class SalaryYear
        {
            public int Year { get; set; }
            public SalaryMonth Salary { get; set; }
        }

        public class Employee
        {

            public List<SalaryYear> SalaryHistory { get; set; }
        }

    }
}
