using MDPActual.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ExcelDataReader;
using System.Data;
using System.IO;

namespace MDPActual.Controllers
{
    public class HomeController : Controller
    {
        public bool test = false;
        public static string fileName = "";
        private string[] path;
        public static string itemName = "";
        public static List<Item> items1;

        [HttpGet]
        public IActionResult Index(List<Item> items = null)
        {
            fileName = "";
            items1 = items == null ? new List<Item>() : items;
            return View(items);
        }
        public IActionResult Index(IFormFile file, [FromServices] Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            //Для теста
            fileName = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";
            path = Directory.GetCurrentDirectory().Split("\\");
            //Для билда
            /*for (int i = 0; i < path.Length - 3; i++)
                fileName += $"{path[i]}\\";
            fileName += $"{@"\wwwroot\files"}" + "\\" + file.FileName;
            Console.WriteLine(fileName);*/

            using (FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            var items = GetItemList();
            return Index(items);
        }
        private List<Item> GetItemList()
        {
            List<Item> students = new List<Item>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                {
                    DataTable specificWorkSheet = reader.AsDataSet().Tables[2];
                    int currentRow = 0;
                    foreach (var row in specificWorkSheet.Rows)
                    {
                        currentRow++;
                        if (currentRow > 5 && ((DataRow)row)[2].ToString() != "")
                            students.Add(new Item()
                            {
                                Name = ((DataRow)row)[0].ToString(),
                                Roll = ((DataRow)row)[1].ToString(),
                                Email = ((DataRow)row)[2].ToString()
                            });
                    }
                }
            }
            return students;
        }
    }
}