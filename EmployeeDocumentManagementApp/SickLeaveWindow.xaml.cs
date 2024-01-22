using System;
using System.Windows;
using OfficeOpenXml;
using System.IO;

namespace EmployeeDocumentManagementApp
{
    public partial class SickLeaveWindow : Window
    {
        public SickLeaveWindow()
        {
            InitializeComponent();
        }

        private void OnSubmitButtonClick(object sender, RoutedEventArgs e)
        {
            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "Болнични2024.xlsx");

            using (var package = File.Exists(filePath) ? new ExcelPackage(new FileInfo(filePath)) : new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Count > 0 ? package.Workbook.Worksheets[0] : package.Workbook.Worksheets.Add("Болнични");

                int lastUsedRow = worksheet.Dimension?.End.Row ?? 1;
                int newRow = lastUsedRow + 1;

                DateTime submissionDateTime = DateTime.Now;

                if (lastUsedRow == 1)
                {
                    worksheet.Cells["A1"].Value = "Служител";
                    worksheet.Cells["B1"].Value = "ЕГН";
                    worksheet.Cells["C1"].Value = "От дата";
                    worksheet.Cells["D1"].Value = "До дата";
                    worksheet.Cells["E1"].Value = "Номер и серия";
                    worksheet.Cells["F1"].Value = "Причина";

                    using (var range = worksheet.Cells["A1:F1"])
                    {
                        range.Style.Font.Bold = true;
                    }
                }

                worksheet.Cells[$"A{newRow}"].Value = txtEmployeeName.Text;
                worksheet.Cells[$"B{newRow}"].Value = txtEGN.Text;
                worksheet.Cells[$"C{newRow}"].Value = dpStartDate.SelectedDate?.ToString("yyyy-MM-dd") ?? string.Empty;
                worksheet.Cells[$"D{newRow}"].Value = dpEndDate.SelectedDate?.ToString("yyyy-MM-dd") ?? string.Empty;
                worksheet.Cells[$"E{newRow}"].Value = txtNumberAndSeries.Text;
                worksheet.Cells[$"F{newRow}"].Value = txtReason.Text;

                worksheet.Cells[$"G{newRow}"].Value = submissionDateTime;
                worksheet.Cells[$"G{newRow}"].Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";

                package.SaveAs(new System.IO.FileInfo(filePath));
            }

            MessageBox.Show("Болничната е записана успешно!");
            Close();
        }
    }
}
