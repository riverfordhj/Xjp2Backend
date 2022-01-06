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
        /// <summary>
        /// 大屏展示
        /// </summary>
        /// <returns></returns>
        /// 
        //获取街道的营收税收
        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetTotalTaR()
        {
            return await _repository.GetTotalTaR().ToListAsync();
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

        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetBuildingFloor(string buildingName)
        {
            return await _repository.GetBuildingFloor(buildingName).ToListAsync();
        }
        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetRoomByBuilding(string buildingName)
        {
            return await _repository.GetRoomByBuilding(buildingName).ToListAsync();
        }



        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetCountTaxByBuilding(string buildingName)
        {
            return await _repository.GetCountTaxByBuilding(buildingName).ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetTaxTop()
        {
            return await _repository.GetTaxTop().ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetTaxTopOnMap()
        {
            return await _repository.GetTaxTopOnMap().ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetCountRevenueByBuilding(string buildingName)
        {
            return await _repository.GetCountRevenueByBuilding(buildingName).ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetRevenueTop()
        {
            return await _repository.GetRevenueTop().ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetRevenueTopOnMap()
        {
            return await _repository.GetRevenueTopOnMap().ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetTotalTaRByBuilding(string buildingName)
        {
            return await _repository.GetTotalTaRByBuilding(buildingName).ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetTotalTaR()
        {
            return await _repository.GetTotalTaR().ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetIndustryTypeByBuilding(string buildingName)
        {
            return await _repository.GetIndustryTypeByBuilding(buildingName).ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetIndustryType()
        {
            return await _repository.GetIndustryType().ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetRevenueRoundByBuilding(string buildingName)
        {
            return await _repository.GetRevenueRoundByBuilding(buildingName).ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetRevenueRound()
        {
            return await _repository.GetRevenueRound().ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetTaxRoundByBuilding(string buildingName)
        {
            return await _repository.GetTaxRoundByBuilding(buildingName).ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetTaxRound()
        {
            return await _repository.GetTaxRound().ToListAsync();
        }

        // GET: api/CompanyBuildings/GetInfoByBuildingNameAndFloor
        [HttpGet("[action]")]
        public async Task<IEnumerable<object>> GetInfoByBuildingNameAndFloor(string buildingName, string floor)
        {
            return await _repository.GetInfoByBuildingAndFloor(buildingName, floor).ToListAsync();
        }

        // GET: api/Companies/GetWholeCompanys_ZH
        [HttpGet("[action]")]
        public async Task<IEnumerable<Object>> GetWholeCompanys_ZH()
        {
            return await _repository.GetWholeCompanys_ZH().ToListAsync();
        }

        // GET: api/Companies/GetCompanysByBuildingWithCH/1
        [HttpGet("[action]/{id}")]
        public IEnumerable<Object> GetCompanysByBuilding_ZH(int id)
        {
            return _repository.GetCompanysByBuilding_ZH(id);

        }
        // 检索公司
        [HttpGet("[action]/{serchName}")]
        public IEnumerable<Object> GetCompanyBySearch(string serchName)//Person
        {
            return _repository.GetCompanyBySearch(serchName);
        }

        // 楼宇地图 展示面板下拉选项
        [HttpGet("[action]/{id}")]
        public async Task<IEnumerable<Object>> GetFloorInfoByBuilding(int id)
        {
            var info = _repository.GetFloorsByBuilding(id);

            return await info.ToListAsync();

        }



    }
}
