using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RESTServer.Data;
using RESTServer.Data.DAO;
using RESTServer.Data.DTO;
using RESTServer.Entities;
using RESTServer.Models;

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
        public ActionResult<IEnumerable<BadEventDTO>> GetEvents()
        {
            // if user is authenticated
            //limit list by .WhereIf(!user.HasAdmin, x => x.UserId == user.Id)

            var dao = new BadEventDAO();
            var list = dao.GetBadEvents(false)// limit here
                .Select(BadEventDTO.Selector().Compile()).ToList();

            //use to populate BadEvents from earlier solution
            //var list_ = _context.Events.ToList();
            //foreach (var badEvent in list_)
            //    dao.CreateBadEvent(badEvent);

            return list;
        }
        // GET: api/BadEvents/archive
        [HttpGet("archive")]
        public ActionResult<IEnumerable<BadEventDTO>> GetArchive()
        {
            // if user is authenticated
            //limit list by .WhereIf(!user.HasAdmin, x => x.UserId == user.Id)

            var dao = new BadEventDAO();
            var list = dao.GetBadEvents(true).Select(BadEventDTO.Selector().Compile()).ToList();// limit here

            return list;
        }

        // GET: api/BadEvents/5
        [HttpGet("{id}")]
        public ActionResult<BadEventDTO> GetBadEvent(int id)
        {
            // check user Access
            var dao = new BadEventDAO();

            var badEvent = dao.GetBadEvent(id);
            
            if (badEvent == null || badEvent.IsDeleted)
                return NotFound();

            return BadEventDTO.Selector().Compile()(badEvent);
        }

        // PUT: api/BadEvents/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public IActionResult PutBadEvent(int id, BadEventDTO dto)
        {
            // check user Access
            if (id != dto.Id)
                return BadRequest();

            var dao = new BadEventDAO();
            var badEvent = dao.GetBadEvent(id);

            if (badEvent == null)
                return NotFound();

            badEvent.Date = dto.Date;
            badEvent.Message = dto.Message;
            badEvent.Placement = dto.Placement;
            badEvent.Reason = dto.Reason;
            badEvent.SeverityId = dto.SeverityId;
            badEvent.StatusId = dto.StatusId;
            badEvent.Modified = DateTime.Now;
            badEvent.ModifiedById = 1; //Exchange for current user

            try
            {
                dao.UpdateBadEvent(badEvent);
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("archive/{id}")]
        public IActionResult PutArchive(int id, BadEventDTO dto)
        {
            // check user Access
            if (id != dto.Id)
                return BadRequest();

            var dao = new BadEventDAO();
            var badEvent = dao.GetBadEvent(id);

            if (badEvent == null)
                return NotFound();

            try
            {
                dao.ArchiveBadEvent(id);
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
        public ActionResult<BadEventDTO> PostBadEvent(BadEventDTO dto)
        {
            //Exchange CreatedByID for current user
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

            var dao = new BadEventDAO();
            var id = dao.CreateBadEvent(badEvent);

            return CreatedAtAction("GetBadEvent", new { id }, BadEventDTO.Selector().Compile()(dao.GetBadEvent(id)));
        }

        // DELETE: api/BadEvents/5
        [HttpDelete("{id}")]
        public ActionResult<bool> DeleteBadEvent(int id)
        {
            //if user is authenticated
            var dao = new BadEventDAO();
            var badEvent = dao.GetBadEvent(id);
            if (badEvent == null)
                return NotFound();

            badEvent.IsDeleted = true;
            dao.UpdateBadEvent(badEvent);
            return true;
        }
    }
}
