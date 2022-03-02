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
    public class 
        ListOfComicsGenreController : ControllerBase
    {
        private readonly comics_lib_dbContext _context;

        public ListOfComicsGenreController(comics_lib_dbContext context)
        {
            _context = context;
        }

        // GET: api/ListOfComicsGenre
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListOfComicsGenre>>> GetListOfComicsGenres()
        {
            return await _context.ListOfComicsGenres.ToListAsync();
        }

        // GET: api/ListOfComicsGenre/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ListOfComicsGenre>> GetListOfComicsGenre(int id)
        {
            var listOfComicsGenre = await _context.ListOfComicsGenres.FindAsync(id);

            if (listOfComicsGenre == null)
            {
                return NotFound();
            }

            return listOfComicsGenre;
        }

        // PUT: api/ListOfComicsGenre/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutListOfComicsGenre(int id, ListOfComicsGenre listOfComicsGenre)
        {
            if (id != listOfComicsGenre.Id)
            {
                return BadRequest();
            }

            _context.Entry(listOfComicsGenre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListOfComicsGenreExists(id))
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

        // POST: api/ListOfComicsGenre
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ListOfComicsGenre>> PostListOfComicsGenre(ListOfComicsGenre listOfComicsGenre)
        {
            _context.ListOfComicsGenres.Add(listOfComicsGenre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetListOfComicsGenre", new { id = listOfComicsGenre.Id }, listOfComicsGenre);
        }

        // DELETE: api/ListOfComicsGenre/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListOfComicsGenre(int id)
        {
            var listOfComicsGenre = await _context.ListOfComicsGenres.FindAsync(id);
            if (listOfComicsGenre == null)
            {
                return NotFound();
            }

            _context.ListOfComicsGenres.Remove(listOfComicsGenre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ListOfComicsGenreExists(int id)
        {
            return _context.ListOfComicsGenres.Any(e => e.Id == id);
        }
        
        // GET: api/ListOfComicsGenre/GetGenres
        [HttpGet]
        [Route("GetGenres")]
        public ActionResult GetGenres(int comicsId)
        {
            try
            {
                var result = _context.ListOfComicsGenres.Where(genre => genre.IdComics == comicsId)
                    .Select(x => x.IdGenreNavigation.Name)
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
