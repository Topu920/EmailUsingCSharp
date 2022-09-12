using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace testmail.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IEmailSender _emailSender;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            get_data();
            var message = new Message(new string[] { "iubatnemesis@gmail.com" }, "Test email", "This is the content from our email.", null);
            _emailSender.SendEmail(message);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<IEnumerable<WeatherForecast>> Post([FromBody] string str)
        {
            var rng = new Random();
            //var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
            var message = new Message(new string[] { "iubatnemesis@gmail.com" }, "Test mail with Attachments", str, null);
            await _emailSender.SendEmailAsync(message);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        private  void get_data()
        {
            
            DataTable dt = new DataTable("Grid");
            Type type = typeof(User);
            PropertyInfo[] NumberOfRecords = type.GetProperties();
            
            foreach (PropertyInfo property in NumberOfRecords)
            {
                dt.Columns.Add(property.Name);
            }
           
            //dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Email"),
            //                            new DataColumn("Name"),

            //                            new DataColumn("Phone") });

            List<User> customers = new List<User>();
            customers = GetlistOfUsers();

            foreach (var customer in customers)
            {
                dt.Rows.Add(customer.Email, customer.Name, customer.Phone);
            }
            string path = "D:\\" + "EmployeeDetails" + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream); 
                    wb.SaveAs(path );

                }
            }
        }

        //private static void get_data()
        //{


        //    try
        //    {
        //        DataTable dt = new DataTable("Grid");
        //        dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Email"),
        //                                        new DataColumn("Name"),

        //                                        new DataColumn("Phone") });

        //        var customers = GetlistOfUsers();

        //        foreach (var customer in customers)
        //        {
        //            dt.Rows.Add(customer.Email, customer.Name, customer.Phone);
        //        }
        //        if (dt.Rows.Count > 0)
        //        {
        //            string path = "D:\\";
        //            if (!Directory.Exists(path))   // CHECK IF THE FOLDER EXISTS. IF NOT, CREATE A NEW FOLDER.
        //            {
        //                Directory.CreateDirectory(path);
        //            }
        //            File.Delete(path + "EmployeeDetails.xlsx"); // DELETE THE FILE BEFORE CREATING A NEW ONE.
        //            Microsoft.Office.Interop.Excel.Application objExcel = new Microsoft.Office.Interop.Excel.Application();
        //            Microsoft.Office.Interop.Excel._Workbook objBook;
        //            Microsoft.Office.Interop.Excel.Worksheet objSheets;
        //            object objOpt = System.Reflection.Missing.Value;
        //            objBook = objExcel.Workbooks.Add(objOpt);
        //            objSheets = (Microsoft.Office.Interop.Excel.Worksheet)objExcel.Worksheets.Add(objOpt, objOpt, objOpt, objOpt);
        //            objSheets.Name = "dffsf";
        //            // ADD A WORKBOOK USING THE EXCEL APPLICATION.
        //            Microsoft.Office.Interop.Excel.Application xlAppToExport = new Microsoft.Office.Interop.Excel.Application();
        //            xlAppToExport.Workbooks.Add();
        //            // ADD A WORKSHEET.
        //            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheetToExport = (Microsoft.Office.Interop.Excel.Worksheet)xlAppToExport.Worksheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
        //            xlWorkSheetToExport.Name = "Result";
        //            // ROW ID FROM WHERE THE DATA STARTS SHOWING.
        //            int iRowCnt = 3;
        //            // SHOW THE HEADER.
        //            xlWorkSheetToExport.Cells[1, 1] = "LAPORAN";
        //            Microsoft.Office.Interop.Excel.Range range = xlWorkSheetToExport.Cells[1, 1] as Microsoft.Office.Interop.Excel.Range;
        //            range.EntireRow.Font.Name = "Calibri";
        //            range.EntireRow.Font.Bold = true;
        //            range.EntireRow.Font.Size = 20;
        //            xlWorkSheetToExport.Range["A1:D1"].MergeCells = true;       // MERGE CELLS OF THE HEADER.
        //                                                                        // SHOW COLUMNS ON THE TOP.
        //            xlWorkSheetToExport.Cells[iRowCnt - 1, 1] = "ID";
        //            xlWorkSheetToExport.Cells[iRowCnt - 1, 2] = "GROUP";
        //            xlWorkSheetToExport.Cells[iRowCnt - 1, 3] = "EMAIL";
        //            int i;
        //            //for (i = 0; i <= dt.Columns.Count - 1; i++)
        //            //{
        //            //    xlWorkSheetToExport.Cells[iRowCnt, i] = dt.Columns[i].ColumnName.ToString();
        //            //}
        //            iRowCnt++;
        //            for (i = 0; i <= dt.Rows.Count - 1; i++)
        //            {
        //                xlWorkSheetToExport.Cells[iRowCnt - 1, 1] = dt.Rows[i][0];
        //                xlWorkSheetToExport.Cells[iRowCnt - 1, 2] = dt.Rows[i][1];
        //                xlWorkSheetToExport.Cells[iRowCnt - 1, 3] = dt.Rows[i][2];
        //                iRowCnt = iRowCnt + 1;
        //            }
        //            // FINALLY, FORMAT THE EXCEL SHEET USING EXCEL'S AUTOFORMAT FUNCTION.
        //            Microsoft.Office.Interop.Excel.Range range1 = xlAppToExport.ActiveCell.Worksheet.Cells[4, 1] as Microsoft.Office.Interop.Excel.Range;
        //            //range1.AutoFormat(ExcelAutoFormat.xlRangeAutoFormatList3);
        //            // SAVE THE FILE IN A FOLDER.
        //            xlWorkSheetToExport.SaveAs(path + "EmployeeDetails.xlsx");
        //            //dt.WriteXml("D:/Tests.xls");
        //            // CLEAR.
        //            xlAppToExport.Workbooks.Close();
        //            xlAppToExport.Quit();
        //            xlAppToExport = null;
        //            xlWorkSheetToExport = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        sda.Dispose();
        //        sda = null;
        //    }
        //}
        // Mimic a database operation
        private List<User> GetlistOfUsers()
        {
            var users = new List<User>()
        {
            new User {
                Email = "mohamad@email.com",
                Name = "Mohamad",
                Phone = "123456"
            },
            new User {
                Email = "donald@email.com",
                Name = "donald",
                Phone = "222222"
            },
            new User {
                Email = "mickey@email.com",
                Name = "mickey",
                Phone = "33333"
            }
        };

            return users;
        }
    }
}
