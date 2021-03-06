#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicsApi.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComicsApi.Models;

namespace ComicsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComicController : ControllerBase
    {
        private readonly comics_lib_dbContext _context;
        private readonly IWebHostEnvironment env;

        public ComicController(comics_lib_dbContext context,IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: api/Comic
        [HttpGet]
        public IActionResult GetComics()
        {
            var result = _context.Comics.Select(comic => new
            {
                id = comic.Id,
                name = comic.Name,
                cover = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host +  comic.Cover,
                date = comic.DateOfIssue,
                description = comic.Description,
                author = comic.IdAuthorNavigation.Name + " " + comic.IdAuthorNavigation.Surname,
                editor = comic.IdEditorNavigation.Name
            }).ToList();
            return new ObjectResult(result);
        }

        // GET: api/Comic/5
        [HttpGet("{id}")]
        public IActionResult GetComic(int id)
        {
            var result = _context.Comics
                .Where(x => x.Id == id)
                .Select(comic => new
            {
                id = comic.Id,
                name = comic.Name,
                cover = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host +  comic.Cover,
                date = comic.DateOfIssue,
                description = comic.Description,
                author = comic.IdAuthorNavigation.Name + " " + comic.IdAuthorNavigation.Surname,
                editor = comic.IdEditorNavigation.Name
            }).FirstOrDefault();
            return new ObjectResult(result);
        }

        // GET: api/Comic/Search
        [HttpGet]
        [Route("Search")]
        public IActionResult Search(string searchRequest)
        {
            var result = _context.Comics
                .Where(x => x.Name.Contains(searchRequest))
                .Select(comic => new
                {
                    id = comic.Id,
                    name = comic.Name,
                    cover = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + comic.Cover,
                    date = comic.DateOfIssue,
                    description = comic.Description,
                    author = comic.IdAuthorNavigation.Name + " " + comic.IdAuthorNavigation.Surname,
                    editor = comic.IdEditorNavigation.Name
                }).ToList();
            return new ObjectResult(result);
        }

        // PUT: api/Comic/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("update")]
        public IActionResult PutComic(ComicsData comicsData)
        {
            if (_context.Comics.Where(i => i.Name == comicsData.Name).Any(e => e.Id != comicsData.Id))
            {
                return new ObjectResult(new { key = "EXIST" });
            }
            else
            {
                var response = _context.Comics.FirstOrDefault(i => i.Id == comicsData.Id);
                response.Name = comicsData.Name;
                response.Description = comicsData.Description;
                response.IdEditor = comicsData.idEditor;
                response.DateOfIssue = Convert.ToDateTime(comicsData.dateOfIssue);
                response.IdAuthor = comicsData.idAuthor;
                _context.SaveChanges();
                return new ObjectResult(new { key = "OK" });
            }
        }

        // POST: api/Comic
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostComic(IFormFile cover)
        {
            string name = Request.Form["name"].ToString();
            DateTime date = Convert.ToDateTime(Request.Form["date"].ToString());
            string description = Request.Form["description"].ToString();
            int idAuthor = Convert.ToInt32(Request.Form["idAuthor"].ToString());
            int idEditor = Convert.ToInt32(Request.Form["idEditor"].ToString());
            if (_context.Comics.Any(i => i.Name == name))
            {
                return new ObjectResult(new { key = "EXIST" });
            }
            else
            {
                if (!Directory.Exists(env.WebRootPath + "\\Covers"))
                {
                    Directory.CreateDirectory(env.WebRootPath + "\\Covers");
                }
                string path = $"\\Covers\\{DateTime.Now.Ticks}_" + cover.FileName;

                using (var fileStream = new FileStream(env.WebRootPath + path, FileMode.Create))
                {
                    await cover.CopyToAsync(fileStream);
                }
                Comic comic = new Comic()
                {
                    DateOfIssue = date,
                    Description = description,
                    IdEditor = idEditor,
                    IdAuthor = idAuthor,
                    Name = name,
                    Cover = path
                };
                _context.Comics.Add(comic);
                _context.SaveChanges();
                var result = _context.Comics.FirstOrDefault(comic => comic.Name == name);
                if (result == null)
                {
                    return new ObjectResult(new { message = "ERROR" });
                }
                else
                {
                    return new ObjectResult(new { message = "ADD" });
                }
            }
        }

        // DELETE: api/Comic/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComic(int id)
        {
            var comic = await _context.Comics.FindAsync(id);
            if (comic == null)
            {
                return NotFound();
            }

            _context.Comics.Remove(comic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComicExists(int id)
        {
            return _context.Comics.Any(e => e.Id == id);
        }
        // GET: api/Comic
        [HttpGet]
        [Route("GetComicsIssue")]
        public IActionResult GetComicsIssue(int idComics)
        {
            var result = _context.ListOfIssues.Where(issue => issue.IdComics==idComics).Select(list => new
            {
                idIssue = list.IdIssue,
                name = list.IdIssueNavigation.NameIssue,
                nameFile = list.IdIssueNavigation.NameFile,
                pathRead = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + list.IdIssueNavigation.PathRead,
                pathDownload = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + list.IdIssueNavigation.PathDownload,
                date = list.IdIssueNavigation.DateOfPublication.ToString(),
                issueNumber = list.IdIssueNavigation.IssueNumber
            }).OrderBy(list=>list.name).ToList();
            return new ObjectResult(result);
        }
    }
}
