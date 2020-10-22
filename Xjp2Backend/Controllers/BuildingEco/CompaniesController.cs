using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsBuildingEconomy.buildingCompany;
using ModelsBuildingEconomy.DataHelper;

namespace Xjp2Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly xjpCompanyContext _context;
        private readonly companyRepository _repository;

       public CompaniesController(xjpCompanyContext context)
        {
            _context = context;// new xjpCompanyContext();
            _repository = new companyRepository(_context);
        }
    
        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Object>>> GetCompany()
        {
            return await _context.Company.ToListAsync();
           
        }

        // GET: api/Companies/GetBuildingCompany
        [HttpGet("[action]")]
        public IEnumerable<Object> GetBuildingCompany()
        {
            return  _repository.GetbuildingEconomy_company();

        }

        // GET: api/Companies/getCompanysByBuilding/1
        [HttpGet("[action]/{id}")]
        public IEnumerable<Object> GetCompanysByBuilding(int id)
        {
            return _repository.GetCompanysByBuilding(id);

        }


        // GET: api/Companies/GetBuildingEcoFields
        [HttpGet("[action]")]
        public IEnumerable<Object> GetBuildingEcoFields()
        {
            return _repository.BuildingEcoFields();

        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var company = await _context.Company.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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

        // POST: api/Companies
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
            _context.Company.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompany", new { id = company.Id }, company);
        }

        //POST: api/Companies/CompanyFields
        [HttpPost("[action]")]
        public async Task<IEnumerable<Object>> CompanyFields([FromBody] CompanyFieldsParameter para)
        {
            CompanyBuilding building = _context.CompanyBuilding.FirstOrDefault(bd => bd.BuildingName == para.BuildingName);
            if(building != null)
            {
                building.BuildingName = para.BuildingName;
            }
            else
            {
                building = new CompanyBuilding
                {
                    BuildingName = para.BuildingName
                };
                _context.CompanyBuilding.Add(building);
            }
             
            Company company = _context.Company.FirstOrDefault(cm => cm.CompanyName == para.CompanyName);
            if(company != null)
            {
                company.BusinessDirection = para.BusinessDirection;
                company.CompanyName = para.CompanyName;
                company.Contacts = para.Contacts;
                company.Phone = para.Phone;
                company.CompanyBuilding = building;
            }
            else
            {
                company = new Company
                {
                    CompanyName = para.CompanyName,
                    Contacts = para.Contacts,
                    Phone = para.Phone,
                    BusinessDirection = para.BusinessDirection,
                    CompanyBuilding = building,
                };
                _context.Company.Add(company);

            }
            
            await _context.SaveChangesAsync();
            return _repository.BuildingEcoFields();
            //return CreatedAtAction("CompanyFields", new { info = "ok" });
        }

        //POST: api/Companies/GetInfoByFloor
        [HttpPost("[action]")]
        public async Task<IEnumerable<Object>> GetInfoByFloor([FromBody] BuildingFloor BF)
        {
            var info = _repository.GetCompanysByFloor(BF.BuildingName, BF.Floor);
            
            return await info.ToListAsync();
       
        }

        // POST: api/Companies/DeleteCompanyByName
        [HttpPost("[action]")]
        public async Task<IEnumerable<Object>> DeleteCompanyByName(string[] arr)
        {
            var company = _context.Company.SingleOrDefault(cm => cm.CompanyName == arr[0]);
            if (company == null)
            {
                return _repository.BuildingEcoFields(); 
            }

            _context.Company.Remove(company);
            await _context.SaveChangesAsync();

            //return company;
            return _repository.BuildingEcoFields();
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Company>> DeleteCompany(int id)
        {
            var company = await _context.Company.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Company.Remove(company);
            await _context.SaveChangesAsync();

            return company;
        }

       

        private bool CompanyExists(int id)
        {
            return _context.Company.Any(e => e.Id == id);
        }
    }
}
