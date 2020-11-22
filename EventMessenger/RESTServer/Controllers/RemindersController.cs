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
            var list = await _context.Reminders.Where(x => !x.IsDeleted).Include(x => x.CreatedBy).ToListAsync();
            var dtoList = new List<ReminderDTO>();

            foreach (var reminder in list)
            {
                var dto = ReminderDTO.Selector().Compile()(reminder);
                dtoList.Add(dto);
            }

            return dtoList;
        }

        // GET: api/Reminders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReminderDTO>> GetReminder(int id)
        {
            var reminder = await _context.Reminders.FindAsync(id);

            if (reminder == null)
            {
                return NotFound();
            }

            var dto = ReminderDTO.Selector().Compile()(reminder);

            var user = await _context.Users.FindAsync(reminder.CreatedById);

            dto.SendtFrom = user.FirstName + " " + user.SirName;

            return dto;
        }

        // PUT: api/Reminders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReminder(int id, [FromBody]ReminderDTO reminder)
        {
            if (id != reminder.Id)
            {
                return BadRequest();
            }
            if (!ReminderExists(id))
            {
                return NotFound();
            }

            var dbObject = _context.Reminders.First(x => x.Id == id);

            dbObject.EventId = reminder.EventId;
            dbObject.Date = reminder.EventDate;
            dbObject.Message = reminder.Message;
            dbObject.Modified = DateTime.Now;
            dbObject.ModifiedById = 1; //Exchange for current user
            _context.Entry(dbObject).State = EntityState.Modified;

            try
            {
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
            var reminder = new Reminder()
            {
                Created = DateTime.Now,
                CreatedById = 1,
                Date = dto.EventDate,
                EventId = dto.EventId,
                Message = dto.Message
            };

            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReminder", new { id = reminder.Id }, ReminderDTO.Selector().Compile()(reminder));
        }

        // DELETE: api/Reminders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteReminder(int id)
        {
            //Only run if user is authenticated
            var reminder = await _context.Reminders.FindAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }

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
