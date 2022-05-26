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
    public class IssueController : ControllerBase
    {
        private readonly comics_lib_dbContext _context;
        private readonly IWebHostEnvironment env;

        public IssueController(comics_lib_dbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: api/Issue
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Issue>>> GetIssues()
        {
            return await _context.Issues.ToListAsync();
        }

        // GET: api/Issue/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Issue>> GetIssue(int id)
        {
            var issue = await _context.Issues.FindAsync(id);

            if (issue == null)
            {
                return NotFound();
            }

            return issue;
        }

        // PUT: api/Issue/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIssue(int id, Issue issue)
        {
            if (id != issue.Id)
            {
                return BadRequest();
            }

            _context.Entry(issue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IssueExists(id))
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

        // POST: api/Issue
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Issue>> PostIssue(Issue issue)
        {
            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIssue", new { id = issue.Id }, issue);
        }

        // DELETE: api/Issue/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIssue(int id)
        {
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null)
            {
                return NotFound();
            }

            _context.Issues.Remove(issue);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IssueExists(int id)
        {
            return _context.Issues.Any(e => e.Id == id);
        }
        
        // GET: api/Issue/GetIssueImages
        [HttpGet]
        [Route("GetIssueImages")]
        public IActionResult GetIssueImages(int issueId)
        {
            var issue = _context.Issues.FirstOrDefault(issue1 => issue1.Id == issueId);
            DirectoryInfo d = new DirectoryInfo(env.WebRootPath + issue.PathRead); 
            FileInfo[] Files = d.GetFiles("*.jpg");
            List<string> images = new List<string>();
            foreach(FileInfo file in Files )
            {
                images.Add(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host +  issue.PathRead  + "\\"+ file.Name);
            }
            return new ObjectResult(images);
        }
        
        // GET: api/Issue/CheckIssue
        [HttpGet]
        [Route("CheckIssue")]
        public IActionResult CheckIssue(int issueNumber, int comicsId)
        {
            var response = _context.ListOfIssues.FirstOrDefault(record => record.IdComics == comicsId
                                                                          && record.IdIssueNavigation.IssueNumber ==
                                                                          issueNumber);
            if (response == null)
            {
                return new ObjectResult(new {key = "OK"});
            }
            else
            {
                return new ObjectResult(new {key = "EXIST"});
            }
        }
        
        // GET: api/Issue/GetIssueFileById
        [HttpGet]
        [Route("GetIssueFileById")]
        public IActionResult GetIssueFileById(int issueId)
        {
            var issue = _context.Issues.FirstOrDefault(issue1 => issue1.Id == issueId);
            return Redirect(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host +  issue.PathDownload);
        }
        
        // GET: api/Issue/GetIssueFileByLink
        [HttpGet]
        [Route("GetIssueFileByLink")]
        public IActionResult GetIssueFileByLink(string link)
        {
            return Redirect(link);
        }
    }
}
