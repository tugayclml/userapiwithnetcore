using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using userapi.Models;

namespace userapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

        private bool UserExists(long id) => _context.User.Any(e => e.Id == id);

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            
            TimeSpan timeSpan = DateTime.Now - DateTime.ParseExact(user.birthday, "mm/dd/yyyy", null);;
            int years = (int) (timeSpan.TotalDays) / 365;
            user.birthday = years.ToString();
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, User updatedUser)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Name = updatedUser.Name;
            user.Surname = updatedUser.Surname;
            user.Picture = updatedUser.Picture;
            TimeSpan timeSpan = DateTime.Now - DateTime.ParseExact(updatedUser.birthday, "mm/dd/yyyy", null);;
            int years = (int) (timeSpan.TotalDays) / 365;
            user.birthday = years.ToString();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(long id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null){
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
