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
    public class UserFavouriteGenreController : ControllerBase
    {
        private readonly comics_lib_dbContext _context;

        public UserFavouriteGenreController(comics_lib_dbContext context)
        {
            _context = context;
        }

        // GET: api/UserFavouriteGenre
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFavouriteGenre>>> GetUserFavouriteGenres()
        {
            return await _context.UserFavouriteGenres.ToListAsync();
        }

        // GET: api/UserFavouriteGenre/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserFavouriteGenre>> GetUserFavouriteGenre(int id)
        {
            var userFavouriteGenre = await _context.UserFavouriteGenres.FindAsync(id);

            if (userFavouriteGenre == null)
            {
                return NotFound();
            }

            return userFavouriteGenre;
        }

        // PUT: api/UserFavouriteGenre/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserFavouriteGenre(int id, UserFavouriteGenre userFavouriteGenre)
        {
            if (id != userFavouriteGenre.Id)
            {
                return BadRequest();
            }

            _context.Entry(userFavouriteGenre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserFavouriteGenreExists(id))
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

        // POST: api/UserFavouriteGenre
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserFavouriteGenre>> PostUserFavouriteGenre(UserFavouriteGenre userFavouriteGenre)
        {
            _context.UserFavouriteGenres.Add(userFavouriteGenre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserFavouriteGenre", new { id = userFavouriteGenre.Id }, userFavouriteGenre);
        }

        // DELETE: api/UserFavouriteGenre/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserFavouriteGenre(int id)
        {
            var userFavouriteGenre = await _context.UserFavouriteGenres.FindAsync(id);
            if (userFavouriteGenre == null)
            {
                return NotFound();
            }

            _context.UserFavouriteGenres.Remove(userFavouriteGenre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserFavouriteGenreExists(int id)
        {
            return _context.UserFavouriteGenres.Any(e => e.Id == id);
        }
        
        // GET: api/UserFavouriteGenre/GetGenre
        [HttpGet]
        [Route("GetGenre")]
        public ActionResult GetGenre(string login)
        {
            
            try
            {
                var currentUser = _context.Users.FirstOrDefault(user => user.Login == login );
                if (currentUser == null)
                {
                    return new ObjectResult(new {message = "ERROR"});
                }
                else
                {
                    
                    var result = _context.UserFavouriteGenres
                        .Where(genre => genre.IdUser == currentUser.Id)
                        .Select(x => x.IdGenreNavigation.Name).ToList();
                    return new ObjectResult(result);
                }
            }
            catch (Exception e)
            {
                return new ObjectResult(new {message = e.Message});
            }
            
        }
        
        
        // POST: api/UserFavouriteGenre/Add
        [HttpPost]
        [Route("Add")]
        public ActionResult AddUserGenre(GenreDataList genreData)
        {
            try
            {
                var currentUser = _context.Users.FirstOrDefault(user => user.Login == genreData.login);
                if (currentUser == null)
                {
                    return new ObjectResult(new {message = "USER_ERROR"});
                }
                else
                {
                    for (int i = 0; i < genreData.genres.Count; i++)
                    {
                        var result = _context.UserFavouriteGenres
                            .FirstOrDefault(genre =>
                                genre.IdGenre == genreData.genres[i].Id && genre.IdUser == currentUser.Id);
                        if (result == null)
                        {
                            UserFavouriteGenre userFavouriteGenre = new UserFavouriteGenre()
                            {
                                IdGenre = genreData.genres[i].Id,
                                IdUser = currentUser.Id
                            };
                            _context.UserFavouriteGenres.Add(userFavouriteGenre);
                        }
                    }
                    _context.SaveChanges();
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
