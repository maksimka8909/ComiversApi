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
    public class TrackedComicController : ControllerBase
    {
        private readonly comics_lib_dbContext _context;

        public TrackedComicController(comics_lib_dbContext context)
        {
            _context = context;
        }

        // GET: api/TrackedComic
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackedComic>>> GetTrackedComics()
        {
            return await _context.TrackedComics.ToListAsync();
        }

        // GET: api/TrackedComic/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrackedComic>> GetTrackedComic(int id)
        {
            var trackedComic = await _context.TrackedComics.FindAsync(id);

            if (trackedComic == null)
            {
                return NotFound();
            }

            return trackedComic;
        }

        // PUT: api/TrackedComic/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrackedComic(int id, TrackedComic trackedComic)
        {
            if (id != trackedComic.Id)
            {
                return BadRequest();
            }

            _context.Entry(trackedComic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrackedComicExists(id))
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

        // POST: api/TrackedComic
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrackedComic>> PostTrackedComic(TrackedComic trackedComic)
        {
            _context.TrackedComics.Add(trackedComic);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrackedComic", new { id = trackedComic.Id }, trackedComic);
        }

        // DELETE: api/TrackedComic/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrackedComic(int id)
        {
            var trackedComic = await _context.TrackedComics.FindAsync(id);
            if (trackedComic == null)
            {
                return NotFound();
            }

            _context.TrackedComics.Remove(trackedComic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrackedComicExists(int id)
        {
            return _context.TrackedComics.Any(e => e.Id == id);
        }
        
        
        // GET: api/TrackedComic/GetComics
        [HttpGet]
        [Route("GetComics")]
        public ActionResult GetComics(string login)
        {
            
            try
            {
                List<TrackedComicsData> tracked = new List<TrackedComicsData>();
                var currentUser = _context.Users.FirstOrDefault(user => user.Login == login );
                if (currentUser == null)
                {
                    return new ObjectResult(new {message = "ERROR"});
                }
                else
                {
                    
                    var result = _context.TrackedComics
                        .Where(bookmark  => bookmark.IdUser == currentUser.Id)
                        .Select(x => x.IdComicsNavigation.Name)
                        .ToList();
                    return new ObjectResult(result);
                }
            }
            catch (Exception e)
            {
                return new ObjectResult(new {message = e.Message});
            }
            
        }
        
        // POST: api/TrackedComic/PutTrack
        [HttpPost]
        [Route("PutTrack")]
        public ActionResult PutTrack(TrackedComicsIdData tracked)
        {
            try
            {
                var isExist = _context.TrackedComics
                    .FirstOrDefault(comics => comics.IdUser == tracked.idUser 
                                              && comics.IdComics == tracked.idComics);
                if (isExist == null)
                {
                    TrackedComic trackedComic = new TrackedComic()
                    {
                        IdComics = tracked.idComics,
                        IdUser = tracked.idUser
                    };
                    _context.TrackedComics.Add(trackedComic);
                    _context.SaveChanges();
                    return new ObjectResult(new {message = "ADD"});
                }
                else
                {
                    _context.TrackedComics.Remove(isExist);
                    _context.SaveChanges();
                    return new ObjectResult(new {message = "REMOVE"});  
                }
            }
            catch (Exception e)
            {
                return new ObjectResult(new {message = e.Message});
            }
        }
    }
}
