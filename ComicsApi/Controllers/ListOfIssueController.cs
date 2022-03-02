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
    public class ListOfIssueController : ControllerBase
    {
        private readonly comics_lib_dbContext _context;

        public ListOfIssueController(comics_lib_dbContext context)
        {
            _context = context;
        }

        // GET: api/ListOfIssue
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListOfIssue>>> GetListOfIssues()
        {
            return await _context.ListOfIssues.ToListAsync();
        }

        // GET: api/ListOfIssue/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ListOfIssue>> GetListOfIssue(int id)
        {
            var listOfIssue = await _context.ListOfIssues.FindAsync(id);

            if (listOfIssue == null)
            {
                return NotFound();
            }

            return listOfIssue;
        }

        // PUT: api/ListOfIssue/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutListOfIssue(int id, ListOfIssue listOfIssue)
        {
            if (id != listOfIssue.Id)
            {
                return BadRequest();
            }

            _context.Entry(listOfIssue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListOfIssueExists(id))
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

        // POST: api/ListOfIssue
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ListOfIssue>> PostListOfIssue(ListOfIssue listOfIssue)
        {
            _context.ListOfIssues.Add(listOfIssue);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetListOfIssue", new { id = listOfIssue.Id }, listOfIssue);
        }

        // DELETE: api/ListOfIssue/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListOfIssue(int id)
        {
            var listOfIssue = await _context.ListOfIssues.FindAsync(id);
            if (listOfIssue == null)
            {
                return NotFound();
            }

            _context.ListOfIssues.Remove(listOfIssue);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ListOfIssueExists(int id)
        {
            return _context.ListOfIssues.Any(e => e.Id == id);
        }
        
        // GET: api/ListOfIssue/Issues
        [HttpGet]
        [Route("Issues")]
        public IActionResult Issues(int comicsId)
        {
            var issues = _context.ListOfIssues
                .Where(issue => issue.IdComics == comicsId)
                .Select(x =>  new
                {
                    id = x.IdIssue, 
                    name = x.IdIssueNavigation.NameIssue,
                    download = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + x.IdIssueNavigation.PathDownload,
                    read = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + x.IdIssueNavigation.PathRead
                }).ToList();
            return new ObjectResult(issues);
        }
    }
}
