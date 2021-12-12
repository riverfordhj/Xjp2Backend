using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DataHelper;
using Nancy.Json;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Text;
using System.Data;

namespace Xjp2Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly StreetContext _context;
        private XjpRepository _repository = null;

        public PersonController(StreetContext xjpContext)//StreetContext context
        {
            _context = xjpContext;// new StreetContext();
            _repository = new XjpRepository(_context);
        }

        //获取渍水点
        // GET: api/People
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Rain>>> GetRainPoint()
        {
            return await _context.Rains.ToListAsync();
        }
        // GET: api/Person/GetCommunitys  获取社区人
        [HttpGet("[action]")]
        public  IEnumerable<object> GetCommunityPersons()
        {
            var userName = GetUserName();
            return  _repository.GetPersonsByCommunity(userName);
        }
        // GET: api/Person/GetCommunitys  获取社区
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetCommunitys()
        {
            var userName = GetUserName();
            return await _repository.GetCommunitysByUserName(userName).ToListAsync();
        }

        // GET: api/Person/GetNetGridInCommuity 获取社区下的网格
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<IEnumerable<NetGrid>>> GetNetGridInCommunity(int id)
        {
            var userName = GetUserName();
            var roleList = GetRolesList(userName);
            var roleName = GetFirstRoleName(roleList);
            if (roleName == "网格员")
            {
                return await _repository.GetNetGridByUserName(userName).ToListAsync();
            }
            else
            {
                return await _repository.GetNetGridInCommunity(id).ToListAsync();
            }
           
        }

        // GET: api/Person/GetBuildingInNetGrid  获取网格下的楼栋
        [HttpGet("[action]/{netid}")]
        public async Task<ActionResult<IEnumerable<Building>>> GetBuildingInNetGrid(int netid)
        {
            return await _repository.GetBuildingInNetGrid(netid).ToListAsync();
        }

        //通过网格查找人
        // GET: api/GetPersonsBySubdivision/1
        [HttpGet("[action]/{id}")]
        public IEnumerable<Object> GetPersonsByNetGrid(int id)//Person
        {
            return _repository.GetPersonsByNetGrid(id);
        }

        //通过小区查找人
        // GET: api/GetPersonsBySubdivision/1
        [HttpGet("[action]/{id}")]
        public IEnumerable<Object> GetPersonsBySubdivision(int id)//Person
        {
            return _repository.GetPersonsBySubdivision(id);
        }

        //通过楼栋查找人
        // GET: api/api/Person/GetPersonsByBuilding_ZH/1
        [HttpGet("[action]/{id}")]
        public IEnumerable<Object> GetPersonsByBuilding(int id)//Person
        {
            return _repository.GetPersonsByBuilding(id);
        }

        // GET: api/Person/GetSubdivsions
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetSubdivsions()
        {
            return await _context.Subdivisions.ToListAsync();
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> GetSubdivsionsByUser()
        {
            var userName = GetUserName();
            return await _repository.GetSubdivsionsByUser(userName).ToListAsync();
        }

        //GET: api/Person/GetBuildingsByNetGrid
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetBuildingsByNetGrid()
        {
            var userName = GetUserName();
            var roleList = GetRolesList(userName);
            var roleName = GetFirstRoleName(roleList);

            if (roleName != "网格员")
            {
                return NotFound();
            }

            return await _repository.GetBuildingsByNetGrid(userName).ToListAsync();

        }
        //GET: api/Person/GetSubdivisionByNetGrid
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetSubdivisionByNetGrid()
        {
            var userName = GetUserName();
            var roleList = GetRolesList(userName);
            var roleName = GetFirstRoleName(roleList);

            if (roleName != "网格员")
            {
                return NotFound();
            }

            return await _repository.GetSubdivisionByNetGrid(userName).ToListAsync();

        }
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetRoomsByBuildingAndNetgrid(string buildingName, string address)
        {
            var userName = GetUserName();
            var roleList = GetRolesList(userName);
            var roleName = GetFirstRoleName(roleList);

            if (roleName != "网格员")
            {
                return NotFound();
            }
            return await _repository.GetRoomsByBuilding(buildingName, address).ToListAsync();
        }

        // GET: api/GetBuildings/1
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<IEnumerable<Building>>> GetBuildingInSubdivision(int id)
        {
            return await _repository.GetBuildingInSubdivision(id).ToListAsync();
        }

        // GET: api/People
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            return await _context.Persons.ToListAsync();
        }

        // GET: api/Person/GetPersonsByUser
        [HttpGet("[action]")]
        public IEnumerable<object> GetPersonsByUser()
        {
            var userName = GetUserName();
            return  _repository.GetPersonsByUser(userName);
        }

        //[HttpPost("[action]")]
        //public IEnumerable<object> GetPersonRoomsByUser_WithBuinldingAndRoom([FromBody])
        //{
        //    var userName = GetUserName();
        //    return _repository.GetPersonRoomsByUser_WithBuinldingAndRoom(userName,buildingName, roomName)
        //}

       //网格员修改指定人员信息
       [HttpPost("[action]")]
        public IEnumerable<object> UpdatePersonHouseByNetGrid([FromBody] PersonUpdateParamTesting personFields)
        {
            var userName = GetUserName();
            var roleList = GetRolesList(userName);
            var roleName = GetFirstRoleName(roleList);

            if (roleName != "网格员")
            {
                return new object[0];//不满足条件，就返回一个空对象数组
            }

            _repository.UpdatePersonHouseByNetGrid(userName, personFields);

            return _repository.GetPersonsByUser(userName);
        }

        //网格员批处理personHouse数据（新建）
        [HttpPost("[action]")]
        public List<object> BatchingPersonHouseData(List<PersonUpdateParamTesting> PersonHouseDatas)
        {
            var userName = GetUserName();

            return _repository.CreatePersonHouse_batching(userName, PersonHouseDatas);
            
        }


        //网格员修改指定人员信息
        [HttpPost("[action]")]
        public void UpdatePersonHouseByNetGrid_void([FromBody] PersonUpdateParamTesting personFields)
        {
            var userName = GetUserName();
            var roleList = GetRolesList(userName);
            var roleName = GetFirstRoleName(roleList);

            if (roleName == "网格员")
            {
                _repository.UpdatePersonHouseByNetGrid(userName, personFields);
            }
        }

        //返回指定网格员提交后数据（未审核）
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<object>>> SearchPersonHouseByNetGrid()
        {
            var userName = GetUserName();
            return await _repository.SearchPersonHouseByNetGrid(userName).ToListAsync();
        }

        //社区审核（确认）网格员的修改
        [HttpPost("[action]")]
        public void VerifyByCommunity([FromBody] VerifyAndConfirmParam verifyFileds)
        {
            var userName = GetUserName();
            var roleList = GetRolesList(userName);
            var roleName = GetFirstRoleName(roleList);

            if (roleName == "社区")
            {
                _repository.VerifyByCommunity(verifyFileds);
            }
            
        }

        //街道批准社区的审核
        [HttpPost("[action]")]
        public IEnumerable<object> ConfirmByAdmin([FromBody] VerifyAndConfirmParam confirmFields)
        {
            var userName = GetUserName();
            var roleList = GetRolesList(userName);

            if (roleList[0].Name != "Administrator" || roleList[1].Name != "网格员")
            {
                return new object[0];//不满足条件，就返回一个空对象数组
            }

           return  _repository.ConfirmByAdmin(confirmFields, userName);
        }

        //返回人房数据的历史编辑数据
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetPersonHouseHistoryInfo()
        {
            //var claimsIdentity = User.Identity as ClaimsIdentity;
            //var userName = claimsIdentity.Name;

            return await _repository.PersonHouseHistoryInfo().ToListAsync();
        }

        #region 公共服务api

        //获取特殊群体，吸毒、信访人员的信息
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Object>>> GetSpecialGroupsType()
        {
            return await _repository.GetSpecialGroupsType().ToListAsync();
        }
        // GET: api/SpecialGroups
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Object>>> GetSpecialGroups()//Person
        {
            var userName = GetUserName();
            return await _repository.GetSpecialGroups(userName).ToListAsync();
        }


        //获取低保户
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Object>>> GetPoorType()
        {
            return await _repository.GetPoorType().ToListAsync();
        }
        // GET: api/SpecialGroups
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Object>>> GetPoorpeople()//Person
        {
            var userName = GetUserName();
            return await _repository.GetPoorpeople(userName).ToListAsync();
        }

        //获取残疾人
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Object>>> GetDisabilityType()
        {
            return await _repository.GetDisabilityType().ToListAsync();
        }
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Object>>> GetDisabilitylevel()
        {
            return await _repository.GetDisabilitylevel().ToListAsync();
        }
        // GET: api/SpecialGroups
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Object>>> GetDisability()//Person
        {
            var userName = GetUserName();
            return await _repository.GetDisability(userName).ToListAsync();
        }

        //获取退伍军人
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Object>>> GetMilitaryType()
        {
            return await _repository.GetMilitaryType().ToListAsync();
        }
        // GET: api/SpecialGroups
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Object>>> GetMilitaryService()//Person
        {
            var userName = GetUserName();
            return await _repository.GetMilitaryService(userName).ToListAsync();
        }

        #endregion


        //获取特殊群体，吸毒、信访人员的位置信息（返回中文数据）
        // GET: api/SpecialGroups
        [HttpGet("[action]")]
        public IEnumerable<Object> GetSpecialPersonLoction_ZH()
        {
            return _repository.GetSpecialPersonLoction_ZH();
        }

        //通过网格查找人（返回中文数据）
        // GET: api/GetPersonsByNetGrid_ZH/1
        [HttpGet("[action]/{id}")]
        public IEnumerable<Object> GetPersonsByNetGrid_ZH(int id)//Person
        {
            return _repository.GetPersonsByNetGrid_ZH(id);
        }

        //通过楼栋查找人（返回中文数据）
        // GET: api/GetPersonsByBuilding_ZH/1
        [HttpGet("[action]/{id}")]
        public IEnumerable<Object> GetPersonsByBuilding_ZH(int id)//Person
        {
            return _repository.GetPersonsByBuilding_ZH(id);
        }


        [HttpGet("[action]")]
        public IEnumerable<Object> GetFields()//Person
        {
            return _repository.GetFields();
        }

        //通过name\身份证、电话号查找人
        // GET: api/GetPersonsBySearch/1
        [HttpGet("[action]/{serchName}")]
        public IEnumerable<Object> GetPersonsBySearch(string serchName)//Person
        {
            var userName = GetUserName();
            return _repository.GetPersonsBySearch(userName,serchName);
        }

        //高级检索
        [HttpPost("[action]")]
         public IEnumerable<Object> GetDataByQuery([FromBody] QueryDataParameterCollection dataForms)//Person
        {
            var userName = GetUserName();
            List<string[]> items = new List<string[]>();
            foreach (var item in dataForms.Items)
            {
                String[] query = { item.Field, item.Operato, item.Sname };
                items.Add(query);
            }
            return _repository.GetDataByQuery(userName,items);
        }


        //通过地址楼栋房间号获取该房间的所有人员信息
        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersonsInRoom([FromBody] PersonInRoomParameter para)
        {
            var coll = _repository.GetPersonsInRoom(para.NetGridName, para.AddressName, para.BuildingName, para.RoomNO);
            if (coll != null)
                return await coll.ToListAsync();
            else
                return NotFound();
        }
        //通过地址楼栋房间号获取roomid
        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRoomId([FromBody] PersonInRoomParameter para)
        {
            var room = await _context.Rooms.Where(item => item.Building.Address == para.AddressName && item.Building.Name == para.BuildingName && item.Name == para.RoomNO).ToListAsync();
            if (room == null)
            {
                return NotFound();
            }

            return room;
        }
 

        // GET: api/Person/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _context.Persons.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        // GET: api/GetRoomByBuilding/5
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRoomByBuilding(int id)
        {
            var rooms = await _context.Rooms.Where(r => r.Building.Id == id).ToListAsync();

            if (rooms == null)
            {
                return NotFound();
            }

            return rooms;
        }

        // PUT: api/People/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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

        // POST: api/People
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Person>> DeletePerson(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return person;
        }

        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }

        //解析Token，返回userName

        private string GetUserName()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Name;
            return userName;
        }
        //根据userName，返回对应的RoleList对象
        private List<Role> GetRolesList(string userName)
        {
            var user = _repository.GetUserByName(userName);
            var roleList = user.Roles;
            return  roleList;
        }

        private string GetFirstRoleName(List<Role> roleList)
        {
            return roleList[0].Name;
        }

    }
}
