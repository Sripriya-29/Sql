using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClaimServices.Models;
using System.Security.Cryptography;

namespace ClaimServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DependentsController : ControllerBase
    {
        private readonly CaseStudyContext _context;

        public DependentsController(CaseStudyContext context)
        {
            _context = context;
        }

        // GET: api/Dependents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dependent>>> GetDependents()
        {
            return await _context.Dependents.ToListAsync();
        }

        // GET: api/Dependents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dependent>> GetDependent(int id)
        {
            var dependent = await _context.Dependents.FindAsync(id);

            if (dependent == null)
            {
                return NotFound();
            }

            return dependent;
        }

        [HttpGet]
        [Route("GetDependent")]
        public async Task<ActionResult<IEnumerable<Dependent>>> GetDependent(string mid)
        {
            if (_context.Dependents == null)
            {
                return NotFound();
            }
            var dependent = await _context.Dependents.Where(x => x.MemberId.ToString() == mid).ToListAsync();
            if (dependent == null)
            {
                return NotFound();
            }



            return dependent;
        }
        // PUT: api/Dependents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutDependent(int id, Dependent dependent)
        //{
        //    if (id != dependent.DependentId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(dependent).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DependentExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        [HttpPut]
        [Route("ubook")]
        public async Task<IActionResult> ubook(int id, DateTime dob)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            if (!DependentExists(id))
            {
                return NotFound();
            }



            try
            {
                Dependent d = await _context.Dependents.Where(x => x.DependentId == id).SingleOrDefaultAsync();



                if (d != null)
                {
                    d.Dob = dob;
                    _context.Entry(d).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                return CreatedAtAction("GetDependents", new { id = d.DependentId }, d);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }



            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDependentId(int id, UpdateDependents dependent)
        {

            var oldDependent = await _context.Dependents.FindAsync(id);
            if (id != oldDependent.DependentId)
            {
                return BadRequest();
            }

            if (dependent.Dob != null)
                oldDependent.Dob = dependent.Dob;
            
            _context.Entry(oldDependent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DependentExists(id))
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

        // POST: api/Dependents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Dependent>> PostDependent(Dependent dependent)
        {

            if (_context.Dependents == null)
            {
                return NotFound();
            }
            var dependents = await _context.Dependents.Where(x => x.MemberId.ToString() == dependent.MemberId.ToString()).ToListAsync();
            if (dependents != null && dependents.Count()>=2 )
            {
                return BadRequest();
            }

            _context.Dependents.Add(dependent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDependent", new { id = dependent.DependentId }, dependent);
        }

        // DELETE: api/Dependents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDependent(int id)
        {
            var dependent = await _context.Dependents.FindAsync(id);
            if (dependent == null)
            {
                return NotFound();
            }

            _context.Dependents.Remove(dependent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DependentExists(int id)
        {
            return _context.Dependents.Any(e => e.DependentId == id);
        }
    }
}
