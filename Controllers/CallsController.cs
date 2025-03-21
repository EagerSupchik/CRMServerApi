using CRMServerApi.Data;
using CRMServerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CallsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/calls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CrmCall>>> GetCalls()
        {
            return await _context.Calls.ToListAsync();
        }

        // GET: api/calls/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CrmCall>> GetCall(int id)
        {
            var call = await _context.Calls.FindAsync(id);
            if (call == null)
                return NotFound();
            return call;
        }

        // POST: api/calls
        [HttpPost]
        public async Task<ActionResult<CrmCall>> PostCall(CrmCall call)
        {
            _context.Calls.Add(call);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCall), new { id = call.Id }, call);
        }

        // PUT: api/calls/{id}
        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> PutCall(int id, CrmCall call)
        {
            if (id != call.Id)
                return BadRequest();

            _context.Entry(call).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CallExists(id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        // DELETE: api/calls/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCall(int id)
        {
            var call = await _context.Calls.FindAsync(id);
            if (call == null)
                return NotFound();

            _context.Calls.Remove(call);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool CallExists(int id)
        {
            return _context.Calls.Any(e => e.Id == id);
        }
    }
}