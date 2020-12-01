using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTServer.Data;
using RESTServer.Data.DTO;
using RESTServer.Entities;

namespace RESTServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BadEventsController : ControllerBase
    {
        private readonly EventDBContext _context;

        public BadEventsController(EventDBContext context)
        {
            _context = context;
        }

        // GET: api/BadEvents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BadEventDTO>>> GetEvents()
        {
            // if user is authenticated

            //limit list by user.HasAdmin ? All : .Where(x => x.CreatedBy == user.Id)

            //fetch all events
            var list = await _context.Events.Where(x => !x.IsDeleted && !x.Archived).ToListAsync();

            var dtoList = new List<BadEventDTO>();

            //select only the needed fields for transfer as described in dto
            foreach (var item in list)
            {
                dtoList.Add(BadEventDTO.Selector().Compile()(item));
            }

            //return dto list
            return dtoList;
        }
        // GET: api/BadEvents/archive
        [HttpGet("archive")]
        public async Task<ActionResult<IEnumerable<BadEventDTO>>> GetArchive()
        {
            //same as above only filtered by the archived

            // if user is authenticated

            //limit list by user.HasAdmin ? All : .Where(x => x.CreatedBy == user.Id)

            var list = await _context.Events.Where(x => !x.IsDeleted && x.Archived).ToListAsync();

            var dtoList = new List<BadEventDTO>();

            foreach (var item in list)
            {
                dtoList.Add(BadEventDTO.Selector().Compile()(item));
            }

            return dtoList;
        }

        // GET: api/BadEvents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BadEventDTO>> GetBadEvent(int id)
        {
            // check user Access

            //fetch event
            var badEvent = await _context.Events.FindAsync(id);

            if (badEvent == null && !badEvent.IsDeleted)
            {
                return NotFound();
            }

            //select only the needed fields for transfer as described in dto
            return BadEventDTO.Selector().Compile()(badEvent);
        }

        // PUT: api/BadEvents/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBadEvent(int id, BadEventDTO dto)
        {
            // check user Access
            //check for a bad request
            if (id != dto.Id)
            {
                return BadRequest();
            }
            //check if the event exists
            if (!BadEventExists(id))
            {
                return NotFound();
            }

            var badEvent = _context.Events.First(x => x.Id == id && !x.IsDeleted && !x.Archived);
            //change the dto fields
            badEvent.Date = dto.Date;
            badEvent.Message = dto.Message;
            badEvent.Placement = dto.Placement;
            badEvent.Reason = dto.Reason;
            badEvent.SeverityId = dto.SeverityId;
            badEvent.StatusId = dto.StatusId;
            badEvent.Modified = DateTime.Now;
            badEvent.ModifiedById = 1; //Exchange for current user

            _context.Entry(badEvent).State = EntityState.Modified;

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

        [HttpPut("archive/{id}")]
        public async Task<IActionResult> PutArchive(int id, BadEventDTO dto)
        {
            // check user Access

            //check for a bad request
            if (id != dto.Id)
            {
                return BadRequest();
            }
            //check if the event exists
            if (!BadEventExists(id))
            {
                return NotFound();
            }

            var badEvent = _context.Events.First(x => x.Id == id);

            //set the event as archived
            badEvent.Archived = true;

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

        // POST: api/BadEvents
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BadEventDTO>> PostBadEvent(BadEventDTO dto)
        {
            //Exchange CreatedByID for current user

            //create new event object
            var badEvent = new BadEvent()
            {
                Created = DateTime.Now,
                CreatedById = 1,
                Date = dto.Date,
                IsDeleted = false,
                Message = dto.Message,
                Placement = dto.Placement,
                Reason = dto.Reason,
                SeverityId = dto.SeverityId,
                StatusId = 1
            };

            //save the event to the db
            _context.Events.Add(badEvent);
            await _context.SaveChangesAsync();

            //return the new object with the new id
            return CreatedAtAction("GetBadEvent", new { id = badEvent.Id }, BadEventDTO.Selector().Compile()(badEvent));
        }

        // DELETE: api/BadEvents/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBadEvent(int id)
        {
            // if user is Authenticated

            //fetch event
            var badEvent = await _context.Events.FindAsync(id);
            if (badEvent == null)
            {
                return NotFound();
            }

            //soft delete event
            badEvent.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        private bool BadEventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
