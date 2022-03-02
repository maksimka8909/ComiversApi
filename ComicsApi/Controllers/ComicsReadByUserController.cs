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
    public class ComicsReadByUserController : ControllerBase
    {
        private readonly comics_lib_dbContext _context;

        public ComicsReadByUserController(comics_lib_dbContext context)
        {
            _context = context;
        }

        // GET: api/ComicsReadByUser
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComicsReadByUser>>> GetComicsReadByUsers()
        {
            return await _context.ComicsReadByUsers.ToListAsync();
        }

        // GET: api/ComicsReadByUser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ComicsReadByUser>> GetComicsReadByUser(int id)
        {
            var comicsReadByUser = await _context.ComicsReadByUsers.FindAsync(id);

            if (comicsReadByUser == null)
            {
                return NotFound();
            }

            return comicsReadByUser;
        }

        // PUT: api/ComicsReadByUser/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComicsReadByUser(int id, ComicsReadByUser comicsReadByUser)
        {
            if (id != comicsReadByUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(comicsReadByUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComicsReadByUserExists(id))
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

        // POST: api/ComicsReadByUser
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ComicsReadByUser>> PostComicsReadByUser(ComicsReadByUser comicsReadByUser)
        {
            _context.ComicsReadByUsers.Add(comicsReadByUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComicsReadByUser", new { id = comicsReadByUser.Id }, comicsReadByUser);
        }

        // DELETE: api/ComicsReadByUser/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComicsReadByUser(int id)
        {
            var comicsReadByUser = await _context.ComicsReadByUsers.FindAsync(id);
            if (comicsReadByUser == null)
            {
                return NotFound();
            }

            _context.ComicsReadByUsers.Remove(comicsReadByUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComicsReadByUserExists(int id)
        {
            return _context.ComicsReadByUsers.Any(e => e.Id == id);
        }
    }
}
