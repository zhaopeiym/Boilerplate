using Boilerplate.Core.Entitys;
using Boilerplate.WebHost.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Boilerplate.WebHost.Controllers
{
    [Route("[controller]/[Action]")]
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
            fileName = fileName.Replace(' ', '_');
            //'A' <'z'
            if (fileName[0] < 'A' || fileName[0] > 'z')
                return "项目名称请以字符开头";
            Regex regex = new Regex(@"^[A-Za-z0-9._\-\[\]]+$");
            if (!regex.IsMatch(fileName))
                return "项目名称只能包含字母、数字、逗号、下划线、中划线等";

            //要被下载的文件路径
            var filePath = Directory.GetCurrentDirectory() + $"/File/DownloadProject/{fileName}.tgz";
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
                        process.StartInfo.Arguments = $" {Directory.GetCurrentDirectory()}/File/boilerplate.sh {fileName}";
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

        [HttpGet]
        public async Task<bool> Init(string key)
        {
            if (key == configuration.GetValue<string>("Key"))
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "bash";
                    process.StartInfo.Arguments = $" {Directory.GetCurrentDirectory()}/File/init.sh";
                    process.Start();
                    process.WaitForExit();
                    process.Close();
                };
            }
            return true;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
