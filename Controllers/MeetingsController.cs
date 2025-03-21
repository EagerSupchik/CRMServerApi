using CRMServerApi.Data;
using CRMServerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MeetingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/meetings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CrmMeeting>>> GetMeetings()
        {
            return await _context.Meetings.ToListAsync();
        }

        // GET: api/meetings/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CrmMeeting>> GetMeeting(int id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting == null)
                return NotFound();
            return meeting;
        }

        // POST: api/meetings
        [HttpPost]
        public async Task<ActionResult<CrmMeeting>> PostMeeting(CrmMeeting meeting)
        {
            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMeeting), new { id = meeting.Id }, meeting);
        }

        // PUT: api/meetings/{id}
        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> PutMeeting(int id, CrmMeeting meeting)
        {
            if (id != meeting.Id)
                return BadRequest();

            _context.Entry(meeting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingExists(id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        // DELETE: api/meetings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeeting(int id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting == null)
                return NotFound();

            _context.Meetings.Remove(meeting);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool MeetingExists(int id)
        {
            return _context.Meetings.Any(e => e.Id == id);
        }
    }
}