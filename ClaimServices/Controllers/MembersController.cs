using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClaimServices.Models;
using Microsoft.CodeAnalysis;

namespace ClaimServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly CaseStudyContext _context;

        public MembersController(CaseStudyContext context)
        {
            _context = context;
        }

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            return await _context.Members.ToListAsync();
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return member;
        }
        [HttpGet]
        [Route("SearchMember")]
        public async Task<ActionResult<IEnumerable<Member>>> SearchBook(string MName)
        {
            if (_context.Members == null)
            {
                return NotFound();
            }
        
            var member = await _context.Members.Where(x => x.Name.Equals(MName)).ToListAsync();
            if (member == null)
            {
                return NotFound();
            }
           
            return member;
        }

        // PUT: api/Members/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMember(int id, Member member)
        //{
        //    if (id != member.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(member).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MemberExists(id))
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMemberId(int id, UpdateMember member)
        {

            var oldMember = await _context.Members.FindAsync(id);
            if (id != oldMember.Id)
            {
                return BadRequest();
            }

            if(member.State!=null)
            oldMember.State = member.State;
            if (member.Email != null)
                if (member.Email.Trim().Length == 0)
                    return BadRequest();

                oldMember.Email = member.Email;
            if (member.Address != null)

                oldMember.Address = member.Address;
            if (member.Pan != null)

                oldMember.Pan = member.Pan;
            if (member.ContactNo != null)

                oldMember.ContactNo = member.ContactNo;

            _context.Entry(oldMember).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
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



        [HttpPut]
        [Route("umember")]
        public async Task<IActionResult> umember(int id, Optional<string> email,string? pan,string? state,string? address,int? contactno)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            if (!MemberExists(id))
            {
                return NotFound();
            }
            try
            {
                Member m= await _context.Members.Where(x => x.Id == id).SingleOrDefaultAsync();
             
                
                var member = await _context.Members.FindAsync(id);

                
                if (email.HasValue && email.Equals == null || pan == null || state == null || address ==null || contactno == null)
                {
                    return BadRequest();
                }


                    await _context.SaveChangesAsync();
                
                if (m!= null)
                {
                    m.Email = email.Value;
                    m.Pan = pan;
                    m.State = state;
                    m.Address = address;
                    m.ContactNo=contactno;
                }
                
                _context.Entry(m).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMember", new { id = m.Id,email=m.Email,pan=m.Pan,state=m.State,address=m.Address,contactno=m.ContactNo }, m);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }



            return NoContent();
        }

        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(Member member)
        {
            member.Password = ClaimServices.Models.EncryptDecrypt.EncodePasswordToBase64(member.Password);
            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMember", new { id = member.Id }, member);
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}
