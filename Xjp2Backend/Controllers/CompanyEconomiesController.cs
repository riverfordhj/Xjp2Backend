using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsBuildingEconomy.buildingCompany;

namespace Xjp2Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyEconomiesController : ControllerBase
    {
        private readonly xjpCompanyContext _context;

        public CompanyEconomiesController()
        {
            _context = new xjpCompanyContext();
        }

        // GET: api/CompanyEconomies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyEconomy>>> GetCompanyEconomy()
        {
            return await _context.CompanyEconomy.ToListAsync();
        }

        // GET: api/CompanyEconomies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyEconomy>> GetCompanyEconomy(int id)
        {
            var companyEconomy = await _context.CompanyEconomy.FindAsync(id);

            if (companyEconomy == null)
            {
                return NotFound();
            }

            return companyEconomy;
        }

        // PUT: api/CompanyEconomies/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompanyEconomy(int id, CompanyEconomy companyEconomy)
        {
            if (id != companyEconomy.Id)
            {
                return BadRequest();
            }

            _context.Entry(companyEconomy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyEconomyExists(id))
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

        // POST: api/CompanyEconomies
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CompanyEconomy>> PostCompanyEconomy(CompanyEconomy companyEconomy)
        {
            _context.CompanyEconomy.Add(companyEconomy);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompanyEconomy", new { id = companyEconomy.Id }, companyEconomy);
        }

        // DELETE: api/CompanyEconomies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CompanyEconomy>> DeleteCompanyEconomy(int id)
        {
            var companyEconomy = await _context.CompanyEconomy.FindAsync(id);
            if (companyEconomy == null)
            {
                return NotFound();
            }

            _context.CompanyEconomy.Remove(companyEconomy);
            await _context.SaveChangesAsync();

            return companyEconomy;
        }

        private bool CompanyEconomyExists(int id)
        {
            return _context.CompanyEconomy.Any(e => e.Id == id);
        }
    }
}
