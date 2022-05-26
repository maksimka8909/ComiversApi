#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComicsApi.Models;

namespace ComicsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly comics_lib_dbContext _context;
        private readonly IWebHostEnvironment env;

        public AuthorController(comics_lib_dbContext context,IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: api/Author
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return await _context.Authors.ToListAsync();
        }

        // GET: api/Author/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // PUT: api/Author/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public IActionResult PutAuthor(int id, string name, string surname, string middleName, string birthday,
            string description)
        {
            if (_context.Authors.Where(e => e.Name ==name && e.Surname == surname && e.MiddleName == middleName).Any(e => e.Id != id))
            {
                return new ObjectResult(new { key = "EXIST" });
            }
            else
            {
                var response = _context.Authors.FirstOrDefault(i => i.Id == id);
                response.Name = name;
                response.Surname = surname;
                response.MiddleName = middleName;
                response.Description = description;
                response.Birthday = Convert.ToDateTime(birthday);
                _context.SaveChanges();
                return new ObjectResult(new { key = "OK" });
            }
        }

        // POST: api/Author
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostAuthor(IFormFile image)
        {
            try
            {
                string surname = Request.Form["surname"].ToString();
                string name = Request.Form["name"].ToString();
                string middleName = Request.Form["middleName"].ToString();
                string description = Request.Form["description"].ToString();
                DateTime birthday = Convert.ToDateTime(Request.Form["birthday"].ToString());
                if (_context.Authors.Any(e => e.Name ==name && e.Surname == surname && e.MiddleName == middleName))
                {
                    return new ObjectResult(new { key = "EXIST" });
                }
                else
                {
                    if (!Directory.Exists(env.WebRootPath + "\\Authors"))
                    {
                        Directory.CreateDirectory(env.WebRootPath + "\\Authors");
                    }
                    string path = $"\\Authors\\{DateTime.Now.Ticks}_" + image.FileName;

                    using (var fileStream = new FileStream(env.WebRootPath + path, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    Author author = new Author()
                    {
                        MiddleName = middleName,
                        Surname = surname,
                        Name = name,
                        Photo = path,
                        Description = description,
                        Birthday = birthday
                    };
                    _context.Authors.Add(author);
                    _context.SaveChanges();
                    return new ObjectResult(new { key = "ADD" });
                }
            }
            catch (Exception e)
            {
                return new ObjectResult(new { key = e.ToString() });
            }
        }

        // DELETE: api/Author/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
     
    }
}
