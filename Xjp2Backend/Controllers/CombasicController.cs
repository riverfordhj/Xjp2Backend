using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelCompany;
using ModelCompany.DataHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xjp2Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CombasicController : ControllerBase
    {
        private readonly CompanyContext _context;
        private readonly comRepo _repository;

        public CombasicController(CompanyContext context)
        {
            _context = context;
            _repository = new comRepo(_context);
        }

        // GET: api/Combasic
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Object>>> GetCompanyBasicInfo()
        {
            return await _context.CompanyBasicInfo.ToListAsync();

        }

        // GET: api/Combasic
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Object>>> GetBuildings()
        {
            return await _context.CompanyBuildings.ToListAsync();

        }

        // GET: api/Companies/getCompanysByBuilding/1
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<IEnumerable<Object>>> GetCompanysByBuilding(int id)
        {
            return await _repository.GetCompanysByBuilding(id).ToListAsync();

        }

        //POST: api/Companies/GetInfoByFloor
        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<Object>>> GetInfoByRoom([FromBody] BuildingFloor BF)
        {
            return await _repository.GetCompanysByRoom(BF.BuildingName, BF.roomName).ToListAsync();
            
        }

        // GET: api/CompanyBuildings/GetInfoByBuildingNameAndFloor
        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetInfoByBuildingNameAndFloor(string buildingName, string floor)
        {
            return await _repository.GetInfoByBuildingAndFloor(buildingName, floor).ToListAsync();
        }

    }
}
