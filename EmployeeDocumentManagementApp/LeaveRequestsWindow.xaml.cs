using System;
using System.Windows;
using OfficeOpenXml;
using System.IO;

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
            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "Отпуски2024.xlsx");
            string employeeName = txtEmployeeName.Text;
            Employee employee = EmployeeRepository.GetEmployeeByName(employeeName);

            if (employee == null)
            {
                MessageBox.Show("Служителят не е намерен. Моля първо го добавете към списъка.");
                return;
            }

            if (employee.RemainingLeaveDays <= 0)
            {
                MessageBox.Show("Служителят е изчерпал своята отпуска за тази година.");
                return;
            }

            employee.RemainingLeaveDays--;

            using (var package = File.Exists(filePath) ? new ExcelPackage(new FileInfo(filePath)) : new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Count > 0 ? package.Workbook.Worksheets[0] : package.Workbook.Worksheets.Add("Отпуски");

                int lastUsedRow = worksheet.Dimension?.End.Row ?? 1;
                int newRow = lastUsedRow + 1;

                DateTime submissionDateTime = DateTime.Now;

                if (lastUsedRow == 1)
                {
                    worksheet.Cells["A1"].Value = "Служител";
                    worksheet.Cells["B1"].Value = "Начална дата";
                    worksheet.Cells["C1"].Value = "Крайна дата";
                    worksheet.Cells["D1"].Value = "Дата и час на подадена заявка";

                    using (var range = worksheet.Cells["A1:D1"])
                    {
                        range.Style.Font.Bold = true;
                    }
                }

                worksheet.Cells[$"A{newRow}"].Value = txtEmployeeName.Text;
                worksheet.Cells[$"B{newRow}"].Value = dpStartDate.SelectedDate?.ToString() ?? string.Empty;
                worksheet.Cells[$"C{newRow}"].Value = dpEndDate.SelectedDate?.ToString() ?? string.Empty;

                worksheet.Cells[$"D{newRow}"].Value = submissionDateTime;
                worksheet.Cells[$"D{newRow}"].Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";

                package.SaveAs(new System.IO.FileInfo(filePath));
            }

            MessageBox.Show("Отпуската е записана успешно!");
            Close();
        }
    }
}
