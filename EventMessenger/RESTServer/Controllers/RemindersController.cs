using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTServer.Data;
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
        public async Task<ActionResult<IEnumerable<ReminderDTO>>> GetReminders()
        {
            //fetch all reminders
            var list = await _context.Reminders.Where(x => !x.IsDeleted).Include(x => x.CreatedBy).ToListAsync();
            var dtoList = new List<ReminderDTO>();

            //select only the needed fields for transfer as described in dto
            foreach (var reminder in list)
            {
                var dto = ReminderDTO.Selector().Compile()(reminder);
                dtoList.Add(dto);
            }

            //return dto list
            return dtoList;
        }

        // GET: api/Reminders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReminderDTO>> GetReminder(int id)
        {
            //fetch the reminder
            var reminder = await _context.Reminders.FindAsync(id);

            if (reminder == null)
            {
                return NotFound();
            }

            //select only the needed fields for transfer as described in dto
            var dto = ReminderDTO.Selector().Compile()(reminder);

            //fetch user details
            var user = await _context.Users.FindAsync(reminder.CreatedById);

            dto.SendtFrom = user.FirstName + " " + user.SirName;

            //return dto
            return dto;
        }

        // PUT: api/Reminders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReminder(int id, [FromBody]ReminderDTO reminder)
        {
            //check for a bad request
            if (id != reminder.Id)
            {
                return BadRequest();
            }
            //check if the reminder exists
            if (!ReminderExists(id))
            {
                return NotFound();
            }

            var dbObject = _context.Reminders.First(x => x.Id == id);

            //change the dto fields
            dbObject.EventId = reminder.EventId;
            dbObject.Date = reminder.EventDate;
            dbObject.Message = reminder.Message;
            dbObject.Modified = DateTime.Now;
            dbObject.ModifiedById = 1; //Exchange for current user
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

        // POST: api/Reminders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ReminderDTO>> PostReminder([FromBody]ReminderDTO dto)
        {
            //Exchange CreatedByID for current user

            //create new reminder object
            var reminder = new Reminder()
            {
                Created = DateTime.Now,
                CreatedById = 1,
                Date = dto.EventDate,
                EventId = dto.EventId,
                Message = dto.Message
            };

            //save reminder to the db
            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync();

            //return the new object with the new id
            return CreatedAtAction("GetReminder", new { id = reminder.Id }, ReminderDTO.Selector().Compile()(reminder));
        }

        // DELETE: api/Reminders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteReminder(int id)
        {
            //Only run if user is authenticated
            
            //fetch reminder 
            var reminder = await _context.Reminders.FindAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }

            //soft delete reminder
            reminder.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        private bool ReminderExists(int id)
        {
            return _context.Reminders.Any(e => e.Id == id);
        }
    }
}
