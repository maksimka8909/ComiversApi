#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComicsApi.Models;
using ComicsApi.Classes;

namespace ComicsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditorController : ControllerBase
    {
        private readonly comics_lib_dbContext _context;
        private readonly IWebHostEnvironment env;

        public EditorController(comics_lib_dbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: api/Editor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Editor>>> GetEditors()
        {
            return await _context.Editors.ToListAsync();
        }

        // GET: api/Editor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Editor>> GetEditor(int id)
        {
            var editor = await _context.Editors.FindAsync(id);

            if (editor == null)
            {
                return NotFound();
            }

            return editor;
        }

        // PUT: api/Editor/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("updateEditor")]
        public IActionResult PutEditor(EditorClass editor)
        {
            try
            {
                if (_context.Editors.Where(i => i.Name == editor.Name).Any(e => e.Id != editor.Id))
                {
                    return new ObjectResult(new { key = "EXIST" });
                }
                else
                {
                    var response = _context.Editors.FirstOrDefault(i => i.Id == editor.Id);
                    response.Name = editor.Name;
                    _context.SaveChanges();
                    return new ObjectResult(new { key = "OK" });
                }
            }
            catch(Exception e)
            {
                return new ObjectResult(new { key = e.Message });
            }
        }

        // POST: api/Editor
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostEditor(IFormFile logo)
        {
            string name = Request.Form["name"].ToString();
            if (_context.Editors.Any(e => e.Name == name))
            {
                return new ObjectResult(new { key = "EXIST" });
            }
            else
            {
                if (!Directory.Exists(env.WebRootPath + "\\Logos"))
                {
                    Directory.CreateDirectory(env.WebRootPath + "\\Logos");
                }
                string path = $"\\Logos\\{DateTime.Now.Ticks}_" + logo.FileName;

                using (var fileStream = new FileStream(env.WebRootPath + path, FileMode.Create))
                {
                    await logo.CopyToAsync(fileStream);
                }
                Editor editor = new Editor()
                {
                    Name = name,
                    Photo = path
                };
                _context.Editors.Add(editor);
                _context.SaveChanges();
                return new ObjectResult(new { key = "ADD" });
            }
        }

        // DELETE: api/Editor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEditor(int id)
        {
            var editor = await _context.Editors.FindAsync(id);
            if (editor == null)
            {
                return NotFound();
            }

            _context.Editors.Remove(editor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EditorExists(int id)
        {
            return _context.Editors.Any(e => e.Id == id);
        }
    }
}
