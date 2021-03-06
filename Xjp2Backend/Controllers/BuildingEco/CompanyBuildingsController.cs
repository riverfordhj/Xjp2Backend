﻿using System;
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
    public class CompanyBuildingsController : ControllerBase
    {
        private readonly xjpCompanyContext _context;

        public CompanyBuildingsController(xjpCompanyContext context)
        {
            _context = context;// new xjpCompanyContext();
        }

        // GET: api/CompanyBuildings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyBuilding>>> GetCompanyBuilding()
        {
            return await _context.CompanyBuilding.ToListAsync();
        }

        // GET: api/CompanyBuildings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyBuilding>> GetCompanyBuilding(int id)
        {
            var companyBuilding = await _context.CompanyBuilding.FindAsync(id);

            if (companyBuilding == null)
            {
                return NotFound();
            }

            return companyBuilding;
        }

        // PUT: api/CompanyBuildings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompanyBuilding(int id, CompanyBuilding companyBuilding)
        {
            if (id != companyBuilding.Id)
            {
                return BadRequest();
            }

            _context.Entry(companyBuilding).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyBuildingExists(id))
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

        // POST: api/CompanyBuildings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CompanyBuilding>> PostCompanyBuilding(CompanyBuilding companyBuilding)
        {
            _context.CompanyBuilding.Add(companyBuilding);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompanyBuilding", new { id = companyBuilding.Id }, companyBuilding);
        }

        // DELETE: api/CompanyBuildings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CompanyBuilding>> DeleteCompanyBuilding(int id)
        {
            var companyBuilding = await _context.CompanyBuilding.FindAsync(id);
            if (companyBuilding == null)
            {
                return NotFound();
            }

            _context.CompanyBuilding.Remove(companyBuilding);
            await _context.SaveChangesAsync();

            return companyBuilding;
        }

        private bool CompanyBuildingExists(int id)
        {
            return _context.CompanyBuilding.Any(e => e.Id == id);
        }
    }
}
