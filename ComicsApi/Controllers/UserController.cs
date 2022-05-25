 #nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
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
    public class UserController : ControllerBase
    {
        private readonly comics_lib_dbContext _context;
        private readonly IWebHostEnvironment env;

        public UserController(comics_lib_dbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult GetUsers()
        {
            var result = _context.Users.Where(user => user.Role == true).Select(user => new
            {
                id = user.Id,
                login = user.Login,
                name = user.Name,
                email = user.Email,
                lastLog = user.LastLog,
                access = user.Access
            }).ToList();
            return new ObjectResult(result);
        }

        // GET: api/User/search
        [HttpGet]
        [Route("Search")]
        public IActionResult UsersSearch(string request)
        {
            var result = _context.Users.Where(user => user.Role == true && user.Login.Contains(request)).Select(user => new
            {
                id = user.Id,
                login = user.Login,
                name = user.Name,
                email = user.Email,
                lastLog = user.LastLog,
                access = user.Access
            }).ToList();
            return new ObjectResult(result);
        }

        // GET: api/User/changestate
        [HttpGet]
        [Route("ChangeState")]
        public IActionResult ChangeState(int id)
        {
            var result = _context.Users.Where(user => user.Id == id).FirstOrDefault();
            if (result != null)
            {
                if (result.Access == true)
                {
                    result.Access = false;
                }
                else
                {
                    result.Access = true;
                }
                _context.Users.Update(result);
                _context.SaveChanges();
                return new ObjectResult(new { result = "OK" });
            }
            else
            {
                return new ObjectResult(new {result="ERROR"});
            }
            
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        
        // GET: api/User/CheckLogin
        [HttpGet]
        [Route("CheckLogin")]
        public ActionResult CheckLogin(string login)
        {
            
            try
            {
                var result = _context.Users.FirstOrDefault(user => user.Login == login);
                if ( result == null)
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
        
        // GET: api/User/CheckEmail
        [HttpGet]
        [Route("CheckEmail")]
        public ActionResult CheckEmail(string email)
        {
            try
            {
                var result = _context.Users.FirstOrDefault(user => user.Email == email);
                if ( result == null)
                {

                    string key = GetKey(6);
                    MailAddress from = new MailAddress("fobos8909@gmail.com", "ComicsLibAgent");
                    MailAddress to = new MailAddress(email);
                    MailMessage m = new MailMessage(from, to);
                    m.Subject = "Подтверждение почты";
                    m.Body = "Вот ваш код для подтверждения почты: " + key;
                    int port = 587;
                    string smpt = "smtp.gmail.com";
                    SmtpClient smtp = new SmtpClient(smpt, port);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("fobos8909@gmail.com", "jcqcnuxhzpvbxzxq");
                    smtp.EnableSsl = true;
                    smtp.Send(m);
                    return new ObjectResult(new {key = CreateMD5(key)});   
                }
                else
                {
                    return new ObjectResult(new {key = "ERROR"});
                    
                }
            }
            catch (Exception e)
            {
                return new ObjectResult(new {message = e.Message});
            }
        }
        
        // POST: api/User/Reg
        [HttpPost]
        [Route("Reg")]
        public async Task<IActionResult> RegistUser()
        {
            try
            {
                IFormFile avatar = Request.Form.Files.FirstOrDefault();
                string login = Request.Form["login"].ToString();
                string password = Request.Form["password"].ToString();
                string email = Request.Form["email"].ToString();
                string name = Request.Form["name"].ToString();
                string path;
                login = login.Replace("\"","");
                password = password.Replace("\"", "");
                email = email.Replace("\"", "");
                name = name.Replace("\"","");
                if (!Directory.Exists(env.WebRootPath + "\\Users"))
                {
                    Directory.CreateDirectory(env.WebRootPath + "\\Users");
                }
                
                if(avatar != null)
                {
                    path = $"\\Users\\" + avatar.FileName;

                    using (var fileStream = new FileStream(env.WebRootPath + path, FileMode.Create))
                    {
                        await avatar.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    path = "\\Users\\Nopicture.png";
                }
                
                User newUser = new User()
                {
                    Login = login,
                    Password = password,
                    Email = email,
                    Photo = path,
                    Name = name
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();
                var result = _context.Users.FirstOrDefault(user => user.Login == login);
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
        
        // POST: api/User/Auth
        [HttpPost]
        [Route("Auth")]
        public ActionResult AuthUser(UserAuthData userAuth)
        {
            try
            {
                var result = _context.Users.FirstOrDefault(user => user.Login == userAuth.Login 
                                                                   && user.Password == userAuth.Password);
                
                return new ObjectResult(result);
                
            }
            catch (Exception e)
            {
                return new ObjectResult(new {message = e.Message});
            }
            
        }
        
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        private string GetKey(int size)
        {
            string key = "";
            Random rnd = new Random();
            for (int i = 0; i < size; i++)
            {
                key += rnd.Next(0, 9).ToString();
            }
            return key;
        }

        // GET: api/User/GetUserImages
        [HttpGet]
        [Route("GetUserImages")]
        public IActionResult GetUserImages()
        {
            DirectoryInfo d = new DirectoryInfo(env.WebRootPath + "//Users"); 

            FileInfo[] Files = d.GetFiles(); 
            List<string> images = new List<string>();

            foreach (FileInfo file in Files)
            {
                images.Add(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "\\Users\\" + file.Name);
            }
            return new ObjectResult(images);
        }

        // GET: api/User/CheckEmail
        [HttpGet]
        [Route("UpdateLog")]
        public ActionResult UpdateLog(int idUser)
        {
            try
            {
                var result = _context.Users.FirstOrDefault(user => user.Id == idUser);
                result.LastLog = DateTime.Now;
                _context.Users.Update(result);
                _context.SaveChanges();
                return new ObjectResult(new { message = "Log updated" });
            }
            catch (Exception e)
            {
                return new ObjectResult(new { message = e.Message });
            }
        }

    }
}
