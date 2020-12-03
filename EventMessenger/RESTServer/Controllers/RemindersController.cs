using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTServer.Data;
using RESTServer.Data.DAO;
using RESTServer.Data.DTO;
using RESTServer.Models;

namespace RESTServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemindersController : ControllerBase
    {
        private readonly EventDBContext _context;

        public RemindersController(EventDBContext context)
        {
            _context = context;
        }

        // GET: api/Reminders
        [HttpGet]
        public ActionResult<IEnumerable<ReminderDTO>> GetReminders()
        {
            //authenticate user
            //set up DAO and fetch all reminders before converting to DTO
            var dao = new ReminderDAO();
            var list = dao.GetReminders() //limit here with .WhereIf(!user.HasAdmin, x => x.UserId == user.Id)
                        .Where(x => !x.IsDeleted)
                        .Select(ReminderDTO.Selector().Compile()).ToList();

            //use to populate BadEvents from earlier solution
            //var list_ = _context.Reminders.ToList();
            //foreach (var reminder in list_)
            //    dao.CreateReminder(reminder);

            return list;
        }

        // GET: api/Reminders/5
        [HttpGet("{id}")]
        public ActionResult<ReminderDTO> GetReminder(int id)
        {
            //authenticate user
            //set up DAO and fetch reminder
            var dao = new ReminderDAO();
            var reminder = dao.GetReminder(id);

            //check existance of reminder
            if (reminder == null || reminder.IsDeleted)
                return NotFound();

            //select only the needed fields for transfer as described in dto
            var dto = ReminderDTO.Selector().Compile()(reminder);

            //fetch userdata for the user that created the reminder
            var userdao = new UserDAO();
            var user = userdao.GetUser(reminder.CreatedById);
            dto.SendtFrom = user.FirstName + " " + user.SirName;

            //return dto
            return dto;
        }

        // PUT: api/Reminders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public IActionResult PutReminder(int id, [FromBody]ReminderDTO reminder)
        {
            //check for a bad request
            if (id != reminder.Id)
                return BadRequest();
            //authenticate user

            //set up DAO and fetch reminder
            var dao = new ReminderDAO();
            var dbObject = dao.GetReminder(id);

            //check existance of reminder
            if (dbObject == null || dbObject.IsDeleted)
                return NotFound();

            //update reminder with DTO fields
            dbObject.EventId = reminder.EventId;
            dbObject.Date = reminder.EventDate;
            dbObject.Message = reminder.Message;
            dbObject.Modified = DateTime.Now;
            dbObject.ModifiedById = 1; //Exchange for current user

            try
            {
                //try to save the reminder
                dao.UpdateReminder(dbObject);
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                //return errorcode if saving fails
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/Reminders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<ReminderDTO> PostReminder([FromBody]ReminderDTO dto)
        {
            //authenticate user
            //Exchange CreatedByID for current user
            //set up DAO and create new reminder
            var dao = new ReminderDAO();
            var reminder = new Reminder()
            {
                Created = DateTime.Now,
                CreatedById = 1,
                Date = dto.EventDate,
                EventId = dto.EventId,
                Message = dto.Message
            };

            //save the reminder to the database
            var id = dao.CreateReminder(reminder);

            //return a created response with the new reminder
            return CreatedAtAction("GetReminder", new { id }, ReminderDTO.Selector().Compile()(dao.GetReminder(id)));
        }

        // DELETE: api/Reminders/5
        [HttpDelete("{id}")]
        public ActionResult<bool> DeleteReminder(int id)
        {
            //authenticate user (CreatedById  = currentuser.Id || admin)
            //set up DAO and fetch reminder
            var dao = new ReminderDAO();
            var reminder = dao.GetReminder(id);
            //check existance of the rminder
            if (reminder == null || reminder.IsDeleted)
                return NotFound();

            //soft delete reminder
            reminder.IsDeleted = true;
            dao.UpdateReminder(reminder);
            return true;
        }
    }
}
