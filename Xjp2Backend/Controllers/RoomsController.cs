using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DataHelper;

namespace Xjp2Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly StreetContext _context;
        private readonly XjpRepository _repositoty;

        public RoomsController(StreetContext context)
        {
            _context = context;
            _repositoty = new XjpRepository(_context);
        }

        // GET: api/Rooms
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            return await _context.Rooms.ToListAsync();
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetRoomsByBuilding(string buildingName, string address)
        {
            return await _repositoty.GetRoomsByBuilding(buildingName, address).ToListAsync();
        }


        // PUT: api/Rooms/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, Room room)
        {
            if (id != room.Id)
            {
                return BadRequest();
            }

            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
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

        // POST: api/Rooms
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Room>> PostRoom(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoom", new { id = room.Id }, room);
        }

        [HttpPost("[action]")]
        public IEnumerable<object> BatchingCreateRooms([FromBody] RoomCreatingParam batchingParam)
        {
            return _repositoty.BatchingRoomsCreating(batchingParam);
        }

        [HttpPost("[action]")]
        public IEnumerable<object> BatchingCreateRoomsWithExcel(List<RoomCreatingParam_Other> RoomsList)
        {
            return _repositoty.BatchingCreateRoomsWithExcel(RoomsList);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<object>>> UpdateTargetRoom( RoomCreatingParam_Other roomData)
        {
            var room = await _context.Rooms.FindAsync(roomData.id);
            return await _repositoty.UpdateRoom(room,roomData).ToListAsync();
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<IEnumerable<object>>> DeleteTargetRoom(RoomCreatingParam_Other roomData)
        {
            var room = await _context.Rooms.FindAsync(roomData.id);
            return await _repositoty.DeleteRoom(room,roomData).ToListAsync();
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Room>> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return room;
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}
