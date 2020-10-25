using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTServer.Data;
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
        public async Task<ActionResult<IEnumerable<BadEvent>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // GET: api/BadEvents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BadEvent>> GetBadEvent(int id)
        {
            var badEvent = await _context.Events.FindAsync(id);

            if (badEvent == null)
            {
                return NotFound();
            }

            return badEvent;
        }

        // PUT: api/BadEvents/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBadEvent(int id, BadEvent badEvent)
        {
            if (id != badEvent.Id)
            {
                return BadRequest();
            }

            _context.Entry(badEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BadEventExists(id))
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

        // POST: api/BadEvents
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BadEvent>> PostBadEvent(BadEvent badEvent)
        {
            _context.Events.Add(badEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBadEvent", new { id = badEvent.Id }, badEvent);
        }

        // DELETE: api/BadEvents/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BadEvent>> DeleteBadEvent(int id)
        {
            var badEvent = await _context.Events.FindAsync(id);
            if (badEvent == null)
            {
                return NotFound();
            }

            _context.Events.Remove(badEvent);
            await _context.SaveChangesAsync();

            return badEvent;
        }

        private bool BadEventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
