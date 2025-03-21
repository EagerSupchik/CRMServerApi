using CRMServerApi.Data;
using CRMServerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DealsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/deals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CrmDeal>>> GetDeals()
        {
            return await _context.Deals.ToListAsync();
        }

        // GET: api/deals/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CrmDeal>> GetDeal(int id)
        {
            var deal = await _context.Deals.FindAsync(id);
            if (deal == null)
                return NotFound();
            return deal;
        }

        // POST: api/deals
        [HttpPost]
        public async Task<ActionResult<CrmDeal>> PostDeal(CrmDeal deal)
        {
            _context.Deals.Add(deal);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDeal), new { id = deal.Id }, deal);
        }

        // PUT: api/deals/{id}
        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> PutDeal(int id, CrmDeal deal)
        {
            if (id != deal.Id)
                return BadRequest();

            _context.Entry(deal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DealExists(id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        // DELETE: api/deals/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeal(int id)
        {
            var deal = await _context.Deals.FindAsync(id);
            if (deal == null)
                return NotFound();

            _context.Deals.Remove(deal);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool DealExists(int id)
        {
            return _context.Deals.Any(e => e.Id == id);
        }
    }
}