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
        public ActionResult<IEnumerable<UserDTO>> GetUsers()
        {
            // if user is authenticated as admin
            //set up DAO and fetch all the users before converting to DTO
            var dao = new UserDAO();
            var list = dao.GetUsers().Select(UserDTO.Selector().Compile()).ToList();

            //use to populate users from earlier solution
            //var list_ = _context.Users.Where(x => !x.IsDeleted).ToList();
            //foreach (var user in list_)
            //{
            //    dao.CreateUser(user);
            //}

            return list;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public ActionResult<UserDTO> GetUser(int id)
        {
            // if user is authenticated and id = currentuser id
            //set up DAO
            var dao = new UserDAO();
            //fetch user
            var user = dao.GetUser(id);
            //check existance of user
            if (user == null || user.IsDeleted)
                return NotFound();

            //select only the needed fields for transfer as described in dto
            return UserDTO.Selector().Compile()(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public IActionResult PutUser(int id, [FromBody]UserDTO user)
        {
            //if current user is Admin
            //set up DAO
            var dao = new UserDAO();
            //check for bad request
            if (id != user.Id)
                return BadRequest();
            //fetch user
            var dbObject = dao.GetUser(id);
            //check existance of user
            if(dbObject == null && !dbObject.IsDeleted)
                return NotFound();
            //update userfields
            dbObject.FirstName = user.FirstName;
            dbObject.SirName = user.SirName;
            dbObject.Password = !string.IsNullOrEmpty(user.Password) ? GetHashedValue(user.Password) : dbObject.Password;
            dbObject.BirthId = !string.IsNullOrEmpty(user.BirthId) ? GetHashedValue(user.BirthId): dbObject.BirthId;
            dbObject.Email = user.Email;
            dbObject.HasAdmin = user.HasAdmin;

            try
            {
                //try to save the user
                dao.UpdateUser(dbObject);
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                //return errorcode if saving fails
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
            //set up DAO
            var dao = new UserDAO();
            //check existance of user
            if (dao.UserExists(GetHashedValue(user.BirthId)))
                return BadRequest();

            //create new user object
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
            //save user
            var id = dao.CreateUser(dbObject);
            //retuen createdresponse with the new user
            return CreatedAtAction("GetUser", new { id }, UserDTO.Selector().Compile()(dao.GetUser(id)));
        }

        //private method to convert a string into a hashed value with SHA-512
        private string GetHashedValue (string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            var builder = new StringBuilder(128);
            using(SHA512 shaM = new SHA512Managed())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                shaM.ComputeHash(bytes);

                foreach(var b in shaM.Hash)
                    builder.Append(b.ToString("X2"));
            }
            return builder.ToString();
        }

        [HttpPost("authenticate")]
        public ActionResult PostAuthenticate ([FromBody] LoginDTO credentials)
        {
            //set up DAO
            var dao = new UserDAO();
            //hash the provided values
            var userName = GetHashedValue(credentials.UserId);
            var pass = GetHashedValue(credentials.Password);
            //fetch all users
            var users = dao.GetUsers();
            //check credentials
            var user = users.FirstOrDefault(x => x.BirthId == userName && x.Password == pass && !x.IsDeleted);
            
            //Add code in this method to create and save a token to return to client
            if (user != null)
                dao.AuthenticateUser(user.AmountOfLogins, user.Id);

            return Ok(user != null);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public ActionResult<bool> DeleteUser(int id)
        {
            // if user is Authenticated and id != currentUser.Id
            var dao = new UserDAO();
            //fetch user and check existance of user
            var user = dao.GetUser(id);
            if (user == null || user.IsDeleted)
                return NotFound();

            //soft delete user
            user.IsDeleted = true;
            dao.UpdateUser(user);
            return true;
        }
    }
}
