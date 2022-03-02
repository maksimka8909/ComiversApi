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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEditor(int id, Editor editor)
        {
            if (id != editor.Id)
            {
                return BadRequest();
            }

            _context.Entry(editor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EditorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Editor
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Editor>> PostEditor(Editor editor)
        {
            _context.Editors.Add(editor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEditor", new { id = editor.Id }, editor);
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
        
        // POST: api/Editor/Reg
        [HttpPost]
        [Route("Reg")]
        public async Task<IActionResult> RegistEditor(IFormFile logo)
        {
            try
            {
                string name = Request.Form["name"].ToString();
                if (!Directory.Exists(env.WebRootPath + "\\Logos"))
                {
                    Directory.CreateDirectory(env.WebRootPath + "\\Logos");
                }
                string path = $"\\Logos\\" + logo.FileName;
                    
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
                var result = _context.Editors.FirstOrDefault(editor => editor.Name == name);
                if (result == null)
                {
                    return new ObjectResult(new {message = "ERROR"});
                }
                else
                {
                    return new ObjectResult(new {message = "OK"});
                }
            }
            catch (Exception e)
            {
                return new ObjectResult(new {message = e.Message});
            }
            
        }
    }
}