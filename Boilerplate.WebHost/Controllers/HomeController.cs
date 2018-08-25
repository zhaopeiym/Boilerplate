using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Boilerplate.WebHost.Models;
using System.IO;
using Microsoft.Extensions.Configuration;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using Boilerplate.Core.Entitys;
using System.Threading.Tasks;

namespace Boilerplate.WebHost.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration configuration;
        public HomeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<object> Download(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                fileName = "temp";
            //要被下载的文件路径
            var filePath = Directory.GetCurrentDirectory() + $"/File/DownloadProject/{fileName}.tar";
            var dbConnection = new MySqlConnection(configuration.GetValue<string>("MySQLSPConnection"));
            var entity = await dbConnection.QueryFirstOrDefaultAsync<BoilerplatePO>("SELECT * from Boilerplates where fileName = @fileName;", new { fileName });

            if (!System.IO.File.Exists(filePath))
            {
                if (entity != null)
                    entity.CreationNumber++;
                try
                {
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = "bash";
                        process.StartInfo.Arguments =$" {Directory.GetCurrentDirectory()}/File/boilerplate.sh {fileName}";
                        process.Start();
                        process.WaitForExit();
                        process.Close();
                    };
                }
                catch (Exception ex)
                {
                    return ex.Message + ex.StackTrace;
                }
            }

            if (entity == null)
            {
                await dbConnection.InsertAsync(new BoilerplatePO()
                {
                    FileName = fileName,
                    DownloadNumber = 1
                });
            }
            else
            {
                entity.DownloadNumber++;
                entity.LastModificationTime = DateTime.Now;
                await dbConnection.UpdateAsync(entity);
            }
            var stream = System.IO.File.OpenRead(filePath);
            return File(stream, "application/octet-stream", Path.GetFileName(filePath));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
