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
    public class ComicsScoreController : ControllerBase
    {
        private readonly comics_lib_dbContext _context;

        public ComicsScoreController(comics_lib_dbContext context)
        {
            _context = context;
        }

        // GET: api/ComicsScore
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComicsScore>>> GetComicsScores()
        {
            return await _context.ComicsScores.ToListAsync();
        }

        // GET: api/ComicsScore/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ComicsScore>> GetComicsScore(int id)
        {
            var comicsScore = await _context.ComicsScores.FindAsync(id);

            if (comicsScore == null)
            {
                return NotFound();
            }

            return comicsScore;
        }

        // PUT: api/ComicsScore/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComicsScore(int id, ComicsScore comicsScore)
        {
            if (id != comicsScore.Id)
            {
                return BadRequest();
            }

            _context.Entry(comicsScore).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComicsScoreExists(id))
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

        // POST: api/ComicsScore
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ComicsScore>> PostComicsScore(ComicsScore comicsScore)
        {
            _context.ComicsScores.Add(comicsScore);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComicsScore", new { id = comicsScore.Id }, comicsScore);
        }

        // DELETE: api/ComicsScore/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComicsScore(int id)
        {
            var comicsScore = await _context.ComicsScores.FindAsync(id);
            if (comicsScore == null)
            {
                return NotFound();
            }

            _context.ComicsScores.Remove(comicsScore);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComicsScoreExists(int id)
        {
            return _context.ComicsScores.Any(e => e.Id == id);
        }
        
        [HttpPost]
        [Route("Send")]
        public IActionResult Send(ComicsScoresData comicsScores)
        {
            var result = _context.ComicsScores
                .FirstOrDefault(score => score.IdUser == comicsScores.IdUser && score.IdComics == comicsScores.IdComics);
            if (result == null)
            {
                ComicsScore comicsScore = new ComicsScore()
                {
                    Mark = comicsScores.Mark,
                    IdComics = comicsScores.IdComics,
                    IdUser = comicsScores.IdUser
                };
                _context.ComicsScores.Add(comicsScore);
                _context.SaveChanges();

                return new ObjectResult(new {key = "ADD"});
            }
            else
            {
                result.Mark = comicsScores.Mark;
                _context.ComicsScores.Add(result);
                _context.SaveChanges();
                return new ObjectResult(new {key = "UPDATE"});                
            }
            
        }
        
        [HttpGet]
        [Route("score")]
        public IActionResult Send(int comicsId)
        {
            int score = 0;
            var result = _context.ComicsScores
                .Where(score => score.IdComics == comicsId)
                .ToList();
            foreach (var i in result)
            {
                score += i.Mark;
            }

            score = score / result.Count;
            return new ObjectResult(new {result = score});
        }
    }
}
