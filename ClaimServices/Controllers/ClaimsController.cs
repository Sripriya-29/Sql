using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClaimServices.Models;

namespace ClaimServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly CaseStudyContext _context;

        public ClaimsController(CaseStudyContext context)
        {
            _context = context;
        }

        // GET: api/Claims
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Claim>>> GetClaims()
        {
            return await _context.Claims.ToListAsync();
        }

        // GET: api/Claims/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Claim>> GetClaim(int id)
        {
            var claim = await _context.Claims.FindAsync(id);

            if (claim == null)
            {
                return NotFound();
            }

            return claim;
        }
        [HttpGet]
        [Route("GetMmeber")]
        public async Task<ActionResult<IEnumerable<Claim>>> GetMemberClaim(string mid)
        {
            if (_context.Claims == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims.Where(x => x.MemberId.ToString() == mid).ToListAsync();
            if (claim == null)
            {
                return NotFound();
            }
            
            return claim;
        }


        // PUT: api/Claims/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClaim(int id, Claim claim)
        {
            if (id != claim.ClaimId)
            {
                return BadRequest();
            }

            _context.Entry(claim).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaimExists(id))
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

        // POST: api/Claims
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Claim>> PostClaim(Claim claim)
        {
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClaim", new { id = claim.ClaimId }, claim);
        }

        // DELETE: api/Claims/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClaim(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            _context.Claims.Remove(claim);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClaimExists(int id)
        {
            return _context.Claims.Any(e => e.ClaimId == id);
        }
    }
}
