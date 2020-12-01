using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RESTServer.Data;
using RESTServer.Data.DTO;
using RESTServer.Entities;

namespace RESTServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EventDBContext _context;

        public UsersController(EventDBContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            //fetch all users
            var list = await _context.Users.Where(x => !x.IsDeleted).ToListAsync();
            var dtoList = new List<UserDTO>();

            //select only the needed fields for transfer as described in dto
            foreach (var user in list)
            {
                var dto = UserDTO.Selector().Compile()(user);
                dtoList.Add(dto);
            }

            //return dto list
            return dtoList;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            //check if the user is found
            if (user == null || user.IsDeleted)
            {
                return NotFound();
            }

            //select only the needed fields for transfer as described in dto
            return UserDTO.Selector().Compile()(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromBody]UserDTO user)
        {
            //if current user is Admin
            //check for a bad request
            if (id != user.Id)
            {
                return BadRequest();
            }

            //check if the user exists
            if (!UserExists(id))
            {
                return NotFound();
            }

            var dbObject = _context.Users.First(x => x.Id == id);

            //change the dto fields
            dbObject.FirstName = user.FirstName;
            dbObject.SirName = user.SirName;
            dbObject.Password = !string.IsNullOrEmpty(user.Password) ? GetHashedValue(user.Password) : dbObject.Password;
            dbObject.BirthId = !string.IsNullOrEmpty(user.Password) ? GetHashedValue(user.BirthId): dbObject.BirthId;
            dbObject.Email = user.Email;
            dbObject.HasAdmin = user.HasAdmin;

            _context.Entry(dbObject).State = EntityState.Modified;

            try
            {
                //try to save the changes
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO user)
        {
            //if user logged in is authenticated

            if (_context.Users.Any(x => x.Email.Equals(user.Email)))
                return BadRequest();

            //create new user object
            var dbObject = new User()
            {
                Active = false,
                AmountOfLogins = 0,
                BirthId = GetHashedValue(user.BirthId),
                Email = user.Email,
                FirstName = user.FirstName,
                HasAdmin = user.HasAdmin,
                IsDeleted = false,
                Password = GetHashedValue(user.Password),
                SirName = user.SirName
            };

            //save user to db
            _context.Users.Add(dbObject);
            await _context.SaveChangesAsync();

            //return the new object with the new id
            return CreatedAtAction("GetUser", new { id = user.Id }, UserDTO.Selector().Compile()(dbObject));
        }

        private string GetHashedValue (string input)
        {
            var builder = new StringBuilder(128);
            using(SHA512 shaM = new SHA512Managed())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                shaM.ComputeHash(bytes);

                foreach(var b in shaM.Hash)
                {
                    builder.Append(b.ToString("X2"));
                }
            }
            return builder.ToString();
        }

        [HttpPost("authenticate")]
        public ActionResult PostAuthenticate ([FromBody] LoginDTO credentials)
        {
            //translate credentials to hashed value
            var user = GetHashedValue(credentials.UserId);
            var pass = GetHashedValue(credentials.Password);

            //check if the user exists
            var result = _context.Users.Any(x => x.Password == pass && x.BirthId == user && !x.IsDeleted);

            if(result)
            {
                _context.Users.First(x => x.Password == pass && x.BirthId == user && !x.IsDeleted).AmountOfLogins++;
            }

            //return bool
            return Ok(result);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteUser(int id)
        {
            // if user is Authenticated and id != currentUser.Id

            //fetch user
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            //soft delete user
            user.IsDeleted = true;

            await _context.SaveChangesAsync();

            return true;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
