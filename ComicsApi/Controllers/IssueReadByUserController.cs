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
    public class IssueReadByUserController : ControllerBase
    {
        private readonly comics_lib_dbContext _context;

        public IssueReadByUserController(comics_lib_dbContext context)
        {
            _context = context;
        }

        // GET: api/IssueReadByUser
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IssueReadByUser>>> GetIssueReadByUsers()
        {
            return await _context.IssueReadByUsers.ToListAsync();
        }

        // GET: api/IssueReadByUser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IssueReadByUser>> GetIssueReadByUser(int id)
        {
            var issueReadByUser = await _context.IssueReadByUsers.FindAsync(id);

            if (issueReadByUser == null)
            {
                return NotFound();
            }

            return issueReadByUser;
        }

        // PUT: api/IssueReadByUser/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIssueReadByUser(int id, IssueReadByUser issueReadByUser)
        {
            if (id != issueReadByUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(issueReadByUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IssueReadByUserExists(id))
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

        // POST: api/IssueReadByUser
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IssueReadByUser>> PostIssueReadByUser(IssueReadByUser issueReadByUser)
        {
            _context.IssueReadByUsers.Add(issueReadByUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIssueReadByUser", new { id = issueReadByUser.Id }, issueReadByUser);
        }

        // DELETE: api/IssueReadByUser/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIssueReadByUser(int id)
        {
            var issueReadByUser = await _context.IssueReadByUsers.FindAsync(id);
            if (issueReadByUser == null)
            {
                return NotFound();
            }

            _context.IssueReadByUsers.Remove(issueReadByUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IssueReadByUserExists(int id)
        {
            return _context.IssueReadByUsers.Any(e => e.Id == id);
        }
        
        // GET: api/IssueReadByUser/GetIssues
        [HttpGet]
        [Route("GetIssues")]
        public ActionResult GetIssues(int userId)
        {
            try
            {
                var result = _context.IssueReadByUsers.Where(user => user.IdUser == userId)
                    .Select(x => x.IdIssue )
                    .ToList();
                return new ObjectResult(result);
            }
            catch (Exception e)
            {
                return new ObjectResult(new {message = e.Message});
            }
        }
    }
}
