﻿using System;
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

            var list = await _context.Events.ToListAsync();

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

            var badEvent = await _context.Events.FindAsync(id);

            if (badEvent == null)
            {
                return NotFound();
            }

            return BadEventDTO.Selector().Compile()(badEvent);
        }

        // PUT: api/BadEvents/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBadEvent(int id, BadEventDTO dto)
        {
            // check user Access
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var badEvent = _context.Events.FirstOrDefault(x => x.Id == id);

            if(badEvent == null)
            {
                return NotFound();
            }

            badEvent.Date = dto.Date;
            badEvent.Message = dto.Message;
            badEvent.Placement = dto.Placement;
            badEvent.Reason = dto.Reason;
            badEvent.SeverityId = dto.SeverityId;
            badEvent.StatusId = dto.StatusId;
            badEvent.Modified = DateTime.Now;
            //badEvent.ModifiedBy = user.Id;

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

        [HttpPut("archive/{id}")]
        public async Task<IActionResult> PutArchive(int id, BadEventDTO dto)
        {
            // check user Access
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var badEvent = _context.Events.FirstOrDefault(x => x.Id == id);

            if (badEvent == null)
            {
                return NotFound();
            }

            badEvent.Archived = true;

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
        public async Task<ActionResult<BadEventDTO>> PostBadEvent(BadEventDTO dto)
        {

            var badEvent = new BadEvent()
            {
                Created = DateTime.Now,
                Date = dto.Date,
                IsDeleted = false,
                Message = dto.Message,
                Placement = dto.Placement,
                Reason = dto.Reason,
                SeverityId = dto.SeverityId,
                StatusId = 1
            };

            _context.Events.Add(badEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBadEvent", new { id = badEvent.Id }, badEvent);
        }

        // DELETE: api/BadEvents/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBadEvent(int id)
        {
            var badEvent = await _context.Events.FindAsync(id);
            if (badEvent == null)
            {
                return NotFound();
            }

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
