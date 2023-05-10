using AngularAuthAPI.Context;
using AngularAuthAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularAuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        public UserController(AppDbContext appDbContext)
        {
            _authContext = appDbContext;
        }




        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();
            var user = await _authContext.Users.FirstOrDefaultAsync(x=>x.Username == userObj.Username && x.Password == userObj.Password);
            if (user == null)
                return NotFound(new { Message = "User Not Found!" });


            return Ok(new { Message = "Login Success!"});

        }




        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            await _authContext.Users.AddAsync(userObj);
            await _authContext.SaveChangesAsync();
            return Ok(new { Message = "User Registered!"});

        }




        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authContext.Users.ToListAsync();
            return Ok(users);
        }






        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _authContext.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { Message = "User Not Found!" });

            return Ok(user);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _authContext.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { Message = "User Not Found!" });

            _authContext.Users.Remove(user);
            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "User Deleted!"
            });
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User userObj)
        {
            if (userObj == null || id != userObj.Id)
                return BadRequest();

            var user = await _authContext.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { Message = "User Not Found!" });

            user.FirstName = userObj.FirstName;
            user.LastName = userObj.LastName;
            user.Username = userObj.Username;
            user.Password = userObj.Password;
            user.Token = userObj.Token;
            user.Role = userObj.Role;
            user.Email = userObj.Email;


            _authContext.Users.Update(user);
            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "User Updated!"
            });
        }




    }
}
