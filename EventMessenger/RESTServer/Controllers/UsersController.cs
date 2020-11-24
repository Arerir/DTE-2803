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
using RESTServer.Data.DAO;
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
            var dao = new UserDAO();

            var list = dao.GetUsers();
            //var list = await _context.Users.Where(x => !x.IsDeleted).ToListAsync(); use to add users from earlier solution

            var dtoList = new List<UserDTO>();

            foreach (var user in list)
            {
                //dao.CreateUser(user);use to add users from earlier solution
                var dto = UserDTO.Selector().Compile()(user);
                dtoList.Add(dto);
            }

            return dtoList;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public ActionResult<UserDTO> GetUser(int id)
        {
            var dao = new UserDAO();

            var user = dao.GetUser(id);

            if (user == null || user.IsDeleted)
            {
                return NotFound();
            }

            return UserDTO.Selector().Compile()(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public IActionResult PutUser(int id, [FromBody]UserDTO user)
        {
            //if current user is Admin

            var dao = new UserDAO();

            if (id != user.Id)
            {
                return BadRequest();
            }

            var dbObject = dao.GetUser(id);

            if(dbObject == null)
            {
                return NotFound();
            }

            dbObject.FirstName = user.FirstName;
            dbObject.SirName = user.SirName;
            dbObject.Password = !string.IsNullOrEmpty(user.Password) ? GetHashedValue(user.Password) : dbObject.Password;
            dbObject.BirthId = !string.IsNullOrEmpty(user.Password) ? GetHashedValue(user.BirthId): dbObject.BirthId;
            dbObject.Email = user.Email;
            dbObject.HasAdmin = user.HasAdmin;

            try
            {
                dao.UpdateUser(dbObject);
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
        public ActionResult<UserDTO> PostUser(UserDTO user)
        {
            //if user logged in is authenticated

            var dao = new UserDAO();

            if (dao.UserExists(GetHashedValue(user.BirthId)))
                return BadRequest();

            var dbObject = new User()
            {
                Id = 0,
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
            var id = dao.CreateUser(dbObject);

            return CreatedAtAction("GetUser", new { id }, UserDTO.Selector().Compile()(dao.GetUser(id)));
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
            var user = GetHashedValue(credentials.UserId);
            var pass = GetHashedValue(credentials.Password);

            var result = _context.Users.Any(x => x.Password == pass && x.BirthId == user && !x.IsDeleted);

            if(result)
            {
                _context.Users.First(x => x.Password == pass && x.BirthId == user && !x.IsDeleted).AmountOfLogins++;
            }

            return Ok(result);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteUser(int id)
        {
            // if user is Authenticated and id != currentUser.Id

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

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
