﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.DataHelper
{
    /// <summary>
    /// 数据访问层，封装访问EF操作
    /// </summary>
    public class XjpRepository
    {
        private StreetContext _context;
        public XjpRepository()
        {
            _context = new StreetContext();
        }

        public XjpRepository(StreetContext context)
        {
            _context = context;
        }

        #region street
        public void AddStreet(StreetUnit street)
        {
            _context.Streets.Add(street);
        }

        public void DeleteStreet(StreetUnit street)
        {
            _context.Streets.Remove(street);
        }

        public StreetUnit GetStreet(int id)
        {
            return _context.Streets.Where(item => item.Id == id).FirstOrDefault();
        }
        public StreetUnit GetStreet(string name)
        {
            return _context.Streets.Where(item => item.Name == name).FirstOrDefault();
        }

        public IEnumerable<StreetUnit> GetStreets()
        {
            return _context.Streets.ToList();//.Take(10)
        }

        public bool StreetExists(int id)
        {
            return _context.Streets.Any(item => item.Id == id);
        }
        #endregion

        #region 社区
        public Community GetCommunity(string name)
        {
            return _context.Communitys.Where(item => item.Name == name).FirstOrDefault();
        }
        #endregion

        #region 小区
        public Subdivision GetSubdivision(string name)
        {
            return _context.Subdivisions.Where(item => item.Name == name || item.Alias.Contains(name)).FirstOrDefault();
        }
        #endregion

        #region building
        public Building GetBuildingInCommunity(string commnunityName, string name)
        {
            //Community com = GetCommunity(commnunityName);
            //if (com != null)
            //    return _context.Buildings.Where(item => item.Community == com && item.Name == name).FirstOrDefault();
            //else
                return null;
        }

        public Building GetBuildingInSubdivision(string subdivisionName, string name)
        {
            Subdivision sub = GetSubdivision(subdivisionName);
            if (sub != null)
            {
                return _context.Buildings.Where(item => item.Subdivision.Id == sub.Id && (item.Name == name || item.Alias == name)).FirstOrDefault();
            }
            else
                return null;
        }

        /// <summary>
        /// 获取小区内的楼栋
        /// </summary>
        /// <param name="id">小区 id</param>
        /// <returns></returns>
        public IQueryable<Building> GetBuildingInSubdivision(int id)
        {
            return _context.Buildings.Where(item => item.Subdivision != null && item.Subdivision.Id == id);
        }
        #endregion

        #region Room
        public Room GetRoom(string subdivisionName, string buidlingName, string roomNO)
        {
            Building building = GetBuildingInSubdivision(subdivisionName, buidlingName);
            if (building != null)
                return _context.Rooms.SingleOrDefault(item => item.Building.Id == building.Id && item.Name == roomNO);
            else
                return null;
        }
        public Room GetRoom(int buidlingId, string roomNO)
        {
            return _context.Rooms.SingleOrDefault(item => item.Building.Id == buidlingId && item.Name == roomNO);
        }
        #endregion

        #region 人员
        /// <summary>
        /// 获取建筑物的住户信息
        /// </summary>
        /// <param name="id">建筑物 id </param>
        /// <returns></returns>
        public IEnumerable<object> GetPersonsByBuilding(int id)
        {
            //HashSet<int> personIDs = _context.PersonRooms.Where(item => item.Room.Building.Id == id).Select(item => item.Person.Id).ToHashSet();
            //return _context.Persons.Where(item => personIDs.Contains(item.Id));

            try
            {
                //根据 room - person 数据
                var roomsWithPersons = from room in _context.Rooms.Where(r => r.Building.Id == id)
                                       from pr in room.PersonRooms
                                       select new
                                       {
                                           RoomId = room.Id,
                                           RoomNO = room.Name,
                                           pr.PersonId,
                                           pr.Person,
                                           IsOwner = pr.IsOwner ? "是" : "否",
                                           IsHouseholder = pr.IsHouseholder ? "是" : "否",
                                           IsLiveHere = pr.IsLiveHere ? "是" : "否",
                                           pr.RelationWithHouseholder,
                                           pr.LodgingReason,
                                           pr.PopulationCharacter                                           
                                       };

                //组连接，附加特殊人群信息
                //var ps = from p in _context.Persons
                //         where p.PersonId == "35220119860918511X"
                //         select new
                //        {
                //            p.PersonId,
                //            p.Name
                //        };
                var psdata = roomsWithPersons.ToList();

                var data = from pr in psdata
                         join sg in _context.SpecialGroups on pr.PersonId equals sg.PersonId into psg // 根据身份证关联
                           select new
                           {
                               pr.RoomId,
                               pr.RoomNO,
                               pr.PersonId,
                               pr.Person,
                               pr.IsOwner,
                               pr.IsHouseholder,
                               pr.IsLiveHere,
                               pr.RelationWithHouseholder,
                               pr.LodgingReason,
                               pr.PopulationCharacter,                               
                               SpecialGroup = psg // 特殊人群信息
                           };

                //var d = data.ToList();// ToLookup(sp => sp.p.PersonId, sp => sp.SpecialGroup);
                //var d1 = data as IEnumerable<object>;
                return data;
            }
            catch(Exception e)
            {
                return null;
            }


        }
        /// <summary>
        /// 根据房间号，获取住房人员信息列表
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public IQueryable<Person> GetPersonsInRoom(int roomId)
        {
            List<int> personIDs = _context.PersonRooms.Where(item => item.Room.Id == roomId).Select(item => item.Person.Id).ToList();
            return _context.Persons.Where(item => personIDs.Contains(item.Id));
        }

        /// <summary>
        /// 通过社区名、楼栋名、房号，获取住房人信息
        /// </summary>
        /// <param name="commnunityName"></param>
        /// <param name="buidlingName"></param>
        /// <param name="roomNO"></param>
        /// <returns></returns>
        public IQueryable<Person> GetPersonsInRoom(string subdivisionName, string buidlingName, string roomNO)
        {
            Room room = GetRoom(subdivisionName, buidlingName, roomNO);

            if (room != null)
            {
                List<int> personIDs = _context.PersonRooms.Where(item => item.Room == room).Select(item => item.Person.Id).ToList();
                return _context.Persons.Where(item => personIDs.Contains(item.Id));
            }
            else
                return null;
        }
        #endregion

        #region 用户、角色
        public User GetUserByName(string name)
        {
            var user = _context.Users.Include(u => u.RoleUsers).ThenInclude(ru => ru.Role).SingleOrDefault(r => r.UserName.Equals(name));
            //加载role信息
            //_context.Entry(user).Collection(u => u.RoleUsers).Load();
            return user;
        }
        #endregion

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
