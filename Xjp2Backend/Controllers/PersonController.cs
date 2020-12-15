﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // GET: api/GetSubdivsions
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Subdivision>>> GetSubdivsions()
        {
            return await _context.Subdivisions.ToListAsync();
                //ToListAsync();
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

        //获取特殊群体，吸毒、信访人员的信息
        // GET: api/SpecialGroups
        [HttpGet("[action]")]
        public IEnumerable<Object> GetSpecialGroups()//Person
        {
            return _repository.GetSpecialGroups();
        }

        //通过楼栋查找人
        // GET: api/GetPersonsByBuilding/1
        [HttpGet("[action]/{id}")]
        public IEnumerable<Object> GetPersonsByBuilding(int id)//Person
        {
            return _repository.GetPersonsByBuilding(id);
        }

        //通过小区查找人
        // GET: api/GetPersonsBySubdivision/1
        [HttpGet("[action]/{id}")]
        public IEnumerable<Object> GetPersonsBySubdivision(int id)//Person
        {
            return _repository.GetPersonsBySubdivision(id);
        }

        [HttpGet("[action]")]
        public IEnumerable<Object> GetFields()//Person
        {
            return _repository.GetFields();
        }

        //通过name\身份证、电话号查找人
        // GET: api/GetPersonsBySearch/1
        [HttpPost("[action]")]
        public IEnumerable<Object> GetPersonsBySearch([FromBody] QueryParameter para)//Person
        {
            return _repository.GetPersonsBySearch(para.SubdivisionId ,para.Name);
        }

        //高级检索
        [HttpPost("[action]")]
         public IEnumerable<Object> GetDataByQuery([FromBody] QueryDataParameterCollection dataForms)//Person
        {
            List<string[]> items = new List<string[]>();
            foreach (var item in dataForms.Items)
            {
                String[] query = { item.Field, item.Operato, item.Sname };
                items.Add(query);
            }
            return _repository.GetDataByQuery(items);
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

        // GET: api/People/5
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
