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
    public class UserBookmarkController : ControllerBase
    {
        private readonly comics_lib_dbContext _context;

        public UserBookmarkController(comics_lib_dbContext context)
        {
            _context = context;
        }

        // GET: api/UserBookmark
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserBookmark>>> GetUserBookmarks()
        {
            return await _context.UserBookmarks.ToListAsync();
        }

        // GET: api/UserBookmark/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserBookmark>> GetUserBookmark(int id)
        {
            var userBookmark = await _context.UserBookmarks.FindAsync(id);

            if (userBookmark == null)
            {
                return NotFound();
            }

            return userBookmark;
        }

        // PUT: api/UserBookmark/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserBookmark(int id, UserBookmark userBookmark)
        {
            if (id != userBookmark.Id)
            {
                return BadRequest();
            }

            _context.Entry(userBookmark).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserBookmarkExists(id))
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

        // POST: api/UserBookmark
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserBookmark>> PostUserBookmark(UserBookmark userBookmark)
        {
            _context.UserBookmarks.Add(userBookmark);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserBookmark", new { id = userBookmark.Id }, userBookmark);
        }

        // DELETE: api/UserBookmark/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserBookmark(int id)
        {
            var userBookmark = await _context.UserBookmarks.FindAsync(id);
            if (userBookmark == null)
            {
                return NotFound();
            }

            _context.UserBookmarks.Remove(userBookmark);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserBookmarkExists(int id)
        {
            return _context.UserBookmarks.Any(e => e.Id == id);
        }
        
        // GET: api/UserBookmark/GetBookmark
        [HttpGet]
        [Route("GetBookmark")]
        public ActionResult GetBookmark(string login)
        {
            
            try
            {
                List<UsersBookmarkData> bookmarks = new List<UsersBookmarkData>();
                var currentUser = _context.Users.FirstOrDefault(user => user.Login == login );
                if (currentUser == null)
                {
                    return new ObjectResult(new {message = "ERROR"});
                }
                else
                {
                    
                    var result = _context.UserBookmarks
                        .Where(bookmark  => bookmark.IdUser == currentUser.Id)
                        .ToList();
                    foreach (var i in result)
                    {
                        var nameComics = _context.ListOfIssues.Where(comics => comics.IdIssue == i.IdIssue)
                            .Select(x => x.IdComicsNavigation.Name).FirstOrDefault();
                        var nameIssue = _context.ListOfIssues.Where(comics => comics.IdIssue == i.IdIssue)
                            .Select(x => x.IdIssueNavigation.NameIssue).FirstOrDefault();
                        UsersBookmarkData usersBookmarkData = new UsersBookmarkData()
                        {
                            comicsName = nameComics,
                            issueName = nameIssue
                        };
                        bookmarks.Add(usersBookmarkData);
                    }
                    return new ObjectResult(bookmarks);
                }
            }
            catch (Exception e)
            {
                return new ObjectResult(new {message = e.Message});
            }
            
        }
        
        // POST: api/UserBookmark/PutBookmark
        [HttpPost]
        [Route("PutBookmark")]
        public ActionResult PutBookmark(UsersBookmarkIdData usersBookmarkIdData)
        {
            try
            {
                var isExist = _context.UserBookmarks
                    .FirstOrDefault(bookmark => bookmark.IdUser == usersBookmarkIdData.idUser && bookmark.IdIssue == usersBookmarkIdData.idIssue);
                if (isExist == null)
                {
                    var comics = _context.ListOfIssues
                        .Where(issue => issue.IdIssue == usersBookmarkIdData.idIssue)
                        .Select(x => x.IdComics)
                        .FirstOrDefault();
                    var listOfIssue = _context.ListOfIssues
                        .Where(issue => issue.IdComics == comics).ToList();
                    foreach (var issue in listOfIssue)
                    {
                        var result = _context.UserBookmarks
                            .FirstOrDefault(bookmark => bookmark.IdUser == usersBookmarkIdData.idUser && bookmark.IdIssue == issue.IdIssue);
                        if (result != null)
                        {
                            result.IdIssue = usersBookmarkIdData.idIssue;
                            _context.SaveChanges();
                            return new ObjectResult(new {message = "UPDATE"});
                        }
                        
                    }

                    UserBookmark userBookmark = new UserBookmark()
                    {
                        IdIssue = usersBookmarkIdData.idIssue,
                        IdUser = usersBookmarkIdData.idUser
                    };
                    _context.UserBookmarks.Add(userBookmark);
                    _context.SaveChanges();
                    return new ObjectResult(new {message = "ADD"});
                }
                else
                {
                    _context.UserBookmarks.Remove(isExist);
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
