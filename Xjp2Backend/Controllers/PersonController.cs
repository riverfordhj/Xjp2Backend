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
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Name;
           
            //var list =  _context.Roles.Where(r => r.Users.Any(u => u.UserName == userName)).Select(u => u);
          

            var list = from userRow in _context.Users.Where(u => u.UserName == userName)
                       from u_RoleUsers in userRow.RoleUsers
                       join ru in _context.RoleUsers
                       on u_RoleUsers.Id equals ru.Id
                       select new
                       {
                           u_RoleUsers.Role.Name
                       };
            var roleList = list.ToList();
 
            if (roleList[0].Name == "网格员")
            {
               var divisions =  from user in _context.Users.Where(u => u.UserName == userName)
                                from grid in user.NetGrid
                                select new 
                                {
                                   grid.Community.Subdivisions[0].Id,
                                   grid.Community.Subdivisions[0].Name
                                };
               return await divisions.ToListAsync();
            }

            if(roleList[0].Name == "水岸星城")
            {
                return await (from comm in _context.Communitys.Where(u => u.Name == "水岸星城")
                              select new 
                              { 
                                 comm.Subdivisions[0].Id,
                                 comm.Subdivisions[0].Name

                              } ).ToListAsync();
            } 

            if (roleList[0].Name == "Administrator" && roleList[1].Name == "网格员")
            {
                return await  _context.Subdivisions.ToListAsync();
            }

            return NotFound();
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
        public async Task<ActionResult<IEnumerable<Person>>> GetPersonsByUser()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Name;

            var divisions = from user in _context.Users.Where(u => u.UserName == userName)
                            from grid in user.NetGrid
                            select grid.Community.Subdivisions;
            return await _context.Persons.ToListAsync();
        }

        //获取特殊群体，吸毒、信访人员的信息
        // GET: api/SpecialGroups
        [HttpGet("[action]")]
        public IEnumerable<Object> GetSpecialGroups()//Person
        {
            return _repository.GetSpecialGroups();
        }

        //通过楼栋查找人
        // GET: api/api/Person/GetPersonsByBuilding_ZH/1
        [HttpGet("[action]/{id}")]
        public IEnumerable<Object> GetPersonsByBuilding(int id)//Person
        {
            return _repository.GetPersonsByBuilding(id);
        }

        //通过楼栋查找人（返回中文数据）
        // GET: api/GetPersonsByBuilding_ZH/1
        [HttpGet("[action]/{id}")]
        public IEnumerable<Object> GetPersonsByBuilding_ZH(int id)//Person
        {
            return _repository.GetPersonsByBuilding_ZH(id);
        }

        //通过小区查找人
        // GET: api/GetPersonsBySubdivision/1
        [HttpGet("[action]/{id}")]
        public IEnumerable<Object> GetPersonsBySubdivision(int id)//Person
        {
            return _repository.GetPersonsBySubdivision(id);
        }

        //通过name\身份证、电话号查找人
        // GET: api/GetPersonsBySearch/1
        [HttpPost("[action]")]
        public IEnumerable<Object> GetPersonsBySearch([FromBody] QueryParameter para)//Person
        {
            return _repository.GetPersonsBySearch(para.SubdivisionId ,para.Name);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersonsInRoom([FromBody] PersonInRoomParameter para)
        {
            var coll = _repository.GetPersonsInRoom(para.SubdivisionName, para.BuildingName, para.RoomNO);
            if (coll != null)
                return await coll.ToListAsync();
            else
                return NotFound();
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
    }
}
