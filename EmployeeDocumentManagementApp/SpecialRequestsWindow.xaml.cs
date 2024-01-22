using System;
using System.Windows;
using OfficeOpenXml;
using System.IO;

namespace EmployeeDocumentManagementApp
{
    public partial class SpecialRequestsWindow : Window
    {
        public SpecialRequestsWindow()
        {
            InitializeComponent();
        }

        private void OnSubmitButtonClick(object sender, RoutedEventArgs e)
        {
            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "Молби2024.xlsx");

            using (var package = File.Exists(filePath) ? new ExcelPackage(new FileInfo(filePath)) : new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Count > 0 ? package.Workbook.Worksheets[0] : package.Workbook.Worksheets.Add("Молби");

                int lastUsedRow = worksheet.Dimension?.End.Row ?? 1;
                int newRow = lastUsedRow + 1;

                DateTime submissionDateTime = DateTime.Now;

                if (lastUsedRow == 1)
                {
                    worksheet.Cells["A1"].Value = "Служител";
                    worksheet.Cells["B1"].Value = "Молба за";

                    using (var range = worksheet.Cells["A1:B1"])
                    {
                        range.Style.Font.Bold = true;
                    }
                }

                worksheet.Cells[$"A{newRow}"].Value = txtEmployeeName.Text;
                worksheet.Cells[$"B{newRow}"].Value = txtRequestFor.Text;

                worksheet.Cells[$"C{newRow}"].Value = submissionDateTime;
                worksheet.Cells[$"C{newRow}"].Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";

                package.SaveAs(new System.IO.FileInfo(filePath));
            }

            MessageBox.Show("Молбата е записана успешно!");
            Close();
        }
    }
}
