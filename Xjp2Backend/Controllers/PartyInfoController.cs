using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xjp2Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Xjp2Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartyInfoController : ControllerBase
    {
        private readonly XjpContext _context;

        public PartyInfoController(XjpContext context)
        {
            _context = context;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        //public IEnumerable<string> GetPartyItems()
        //{
        //    return new string[] { "value1", "value2" };
        //}
        public async Task<ActionResult<IEnumerable<PartyInfo>>> GetPartyItems()
        {
            return await _context.PartyInfos.ToListAsync();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        //public string GetPartyItem(int id)
        //{
        //    return "value";
        //}
        public async Task<ActionResult<PartyInfo>> GetPartyItem(long id)
        {
            var todoItem = await _context.PartyInfos.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // POST api/<ValuesController>
        [HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}
        public async Task<ActionResult<PartyInfo>> PostPartyItem(PartyInfo item)
        {
            _context.PartyInfos.Add(item);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return CreatedAtAction(nameof(GetPartyItem), new { id = item.Id }, item);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}
        public async Task<IActionResult> PutPartyItem(long id, PartyInfo item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartyItemExists(id))
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

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
        public async Task<ActionResult<PartyInfo>> DeleteTodoItem(long id)
        {
            var item = await _context.PartyInfos.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.PartyInfos.Remove(item);
            await _context.SaveChangesAsync();

            return item;
        }

        private bool PartyItemExists(long id)
        {
            return _context.PartyInfos.Any(e => e.Id == id);
        }
    }
}
