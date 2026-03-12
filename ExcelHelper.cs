using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;

namespace QR1000Reader
{
    public static class ExcelHelper
    {
        public static void ExportToExcel(List<PassengerRecord> records, string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                // 获取今天日期（两位数格式，如"10"）
                string today = DateTime.Today.ToString("yyyy-MM-dd");
                string dayOfMonth = DateTime.Today.ToString("dd");
                
                // 创建工作表，名称为日期两位数
                var worksheet = workbook.Worksheets.Add(dayOfMonth);
                
                // A1 显示日期两位数
                worksheet.Cell(1, 1).Value = dayOfMonth;
                
                // A2:K2 英文表头
                worksheet.Cell(2, 1).Value = "ID";
                worksheet.Cell(2, 2).Value = "FPORTCODE";
                worksheet.Cell(2, 3).Value = "TPORTCODE";
                worksheet.Cell(2, 4).Value = "FPORTNAME";
                worksheet.Cell(2, 5).Value = "TPORTNAME";
                worksheet.Cell(2, 6).Value = "SETOFFDATE";
                worksheet.Cell(2, 7).Value = "SETOFFTIME";
                worksheet.Cell(2, 8).Value = "TICKETCODE";
                worksheet.Cell(2, 9).Value = "IDTYPE";
                worksheet.Cell(2, 10).Value = "IDNUMBER";
                worksheet.Cell(2, 11).Value = "NAME";
                
                // A3:K3 中文表头
                worksheet.Cell(3, 1).Value = "序号";
                worksheet.Cell(3, 2).Value = "始发港代码*";
                worksheet.Cell(3, 3).Value = "到达港代码*";
                worksheet.Cell(3, 4).Value = "始发港名称*";
                worksheet.Cell(3, 5).Value = "到达港名称*";
                worksheet.Cell(3, 6).Value = "航班日期*";
                worksheet.Cell(3, 7).Value = "航班时间*";
                worksheet.Cell(3, 8).Value = "票号";
                worksheet.Cell(3, 9).Value = "证件类型";
                worksheet.Cell(3, 10).Value = "证件号码";
                worksheet.Cell(3, 11).Value = "旅客姓名*";
                
                // 设置表头样式（A2:K3）
                var headerRange = worksheet.Range(2, 1, 3, 11);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                
                // 从 A4 开始填充数据
                int row = 4;
                foreach (var record in records)
                {
                    worksheet.Cell(row, 1).Value = record.Id;
                    worksheet.Cell(row, 2).Value = record.DeparturePortCode;
                    worksheet.Cell(row, 3).Value = record.ArrivalPortCode;
                    worksheet.Cell(row, 4).Value = record.DeparturePortName;
                    worksheet.Cell(row, 5).Value = record.ArrivalPortName;
                    worksheet.Cell(row, 6).Value = record.FlightDate.ToString("yyyy-MM-dd");
                    worksheet.Cell(row, 7).Value = record.FlightTime;
                    worksheet.Cell(row, 8).Value = record.TicketNumber;
                    worksheet.Cell(row, 9).Value = record.DocumentType;
                    worksheet.Cell(row, 10).Value = record.DocumentNumber;
                    worksheet.Cell(row, 11).Value = record.PassengerName;
                    row++;
                }

                // 自动调整列宽
                worksheet.Columns(1, 11).AdjustToContents();
                
                // 设置列宽最小值
                worksheet.Column(1).Width = 8;
                worksheet.Column(2).Width = 12;
                worksheet.Column(3).Width = 12;
                worksheet.Column(4).Width = 15;
                worksheet.Column(5).Width = 15;
                worksheet.Column(6).Width = 12;
                worksheet.Column(7).Width = 10;
                worksheet.Column(8).Width = 15;
                worksheet.Column(9).Width = 15;
                worksheet.Column(10).Width = 20;
                worksheet.Column(11).Width = 15;

                // 保存文件
                workbook.SaveAs(filePath);
            }
        }
    }
}
