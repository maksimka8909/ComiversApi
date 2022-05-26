using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using ComicsApi.Classes;
using ComicsApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace ComicsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly comics_lib_dbContext _context;
        private readonly IWebHostEnvironment env;
        
        public DownloadController(comics_lib_dbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // POST: api/download/addfile
        [HttpPost]
        [Route("AddFile")]
        public async Task<IActionResult> AddFile(IFormFile file)
        {
            int comicsId = Int32.Parse(Request.Form["comicsId"]);
            string issueName = Request.Form["issueName"].ToString();
            int issueNumber = Int32.Parse(Request.Form["issueNumber"]);
            try
            {
                if (!Directory.Exists(env.WebRootPath + $"\\Download\\{comicsId.ToString()}"))
                {
                    Directory.CreateDirectory(env.WebRootPath + $"\\Download\\{comicsId}");
                }
                string path = "\\Download\\" + comicsId + $"\\{comicsId}_{issueNumber}_" + file.FileName;
                    
                using (var fileStream = new FileStream(env.WebRootPath + path, FileMode.Create,FileAccess.Write))
                {
                     await file.CopyToAsync(fileStream);
                }
                if (!Directory.Exists(env.WebRootPath + $"\\Comics\\{comicsId.ToString()}\\{comicsId}_{issueNumber}_{Path.GetFileNameWithoutExtension(file.FileName)}"))
                {
                    Directory.CreateDirectory(env.WebRootPath + $"\\Comics\\{comicsId}\\{comicsId}_{issueNumber}_{Path.GetFileNameWithoutExtension(file.FileName)}");
                }
                string targetFolder = env.WebRootPath + $"\\Comics\\{comicsId}\\{comicsId}_{issueNumber}_{Path.GetFileNameWithoutExtension(file.FileName)}"; // папка, куда распаковывается файл
                string zipFile = env.WebRootPath + $"\\Download\\{comicsId}\\{comicsId}_{issueNumber}_{file.FileName}"; // сжатый файл
                var result = Path.ChangeExtension(zipFile, ".zip");
                System.IO.File.Move(zipFile, zipFile.Replace(Path.GetExtension(zipFile), ".zip"));
                ZipFile.ExtractToDirectory(result, targetFolder);
                Issue issue = new Issue()
                {
                    NameFile = $"{comicsId}_{issueNumber}_"+Path.ChangeExtension(file.FileName, ".zip"),
                    PathRead = $"\\Comics\\{comicsId}\\{comicsId}_{issueNumber}_{Path.GetFileNameWithoutExtension(file.FileName)}",
                    PathDownload = Path.ChangeExtension($"\\Download\\{comicsId}\\{comicsId}_{issueNumber}_{file.FileName}", ".zip"),
                    NameIssue = issueName,
                    IssueNumber = issueNumber
                };
                _context.Issues.Add(issue);
                _context.SaveChanges();
                var newIssue = _context.Issues.FirstOrDefault(issue1 => issue1.NameFile == $"{comicsId}_{issueNumber}_"+Path.ChangeExtension(file.FileName, ".zip") &&
                                                                        issue1.NameIssue == issueName && 
                                                                        issue1.PathDownload == Path.ChangeExtension($"\\Download\\{comicsId}\\{comicsId}_{issueNumber}_{file.FileName}", ".zip") && 
                                                                        issue1.PathRead == $"\\Comics\\{comicsId}\\{comicsId}_{issueNumber}_{Path.GetFileNameWithoutExtension(file.FileName)}" &&
                                                                        issue1.IssueNumber == issueNumber
                                                                        );
                ListOfIssue listOfIssue = new ListOfIssue()
                {
                    IdComics = comicsId,
                    IdIssue = newIssue.Id
                };
                _context.ListOfIssues.Add(listOfIssue);
                _context.SaveChanges();
                return new ObjectResult(new {key = "OK"});
            }
            catch (Exception e)
            {
                return new ObjectResult(new {key = e.Message});
            }
        }

    }
}