using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

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
        //public Building GetBuildingInCommunity(string commnunityName, string name)
        //{
        //Community com = GetCommunity(commnunityName);
        //if (com != null)
        //    return _context.Buildings.Where(item => item.Community == com && item.Name == name).FirstOrDefault();
        //else
        //   return null;
        // }

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
                                           BulidingName = room.Building.Name,
                                           SubdivsionName = room.Building.Subdivision.Name,
                                           CommunityName = room.Building.Subdivision.Community.Name,
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
                               pr.CommunityName,
                               pr.SubdivsionName,
                               pr.BulidingName,
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
            catch (Exception e)
            {
                return null;
            }


        }
        ///<summary>
        ///
        /// 根据小区获得人员信息表
        /// 
        ///</summary>
        public IEnumerable<object> GetPersonsBySubdivision(int id)
        {
            try
            {
                //根据 room - person 数据
                var roomsWithPersons = from room in _context.Rooms.Where(r => r.Building.Subdivision.Id == id)
                                       from pr in room.PersonRooms
                                       select new
                                       {
                                           RoomId = room.Id,
                                           RoomNO = room.Name,
                                           BulidingName = pr.Room.Building.Name,
                                           SubdivsionName = pr.Room.Building.Subdivision.Name,
                                           CommunityName = pr.Room.Building.Subdivision.Community.Name,
                                           pr.PersonId,
                                           pr.Person,
                                           IsOwner = pr.IsOwner ? "是" : "否",
                                           IsHouseholder = pr.IsHouseholder ? "是" : "否",
                                           IsLiveHere = pr.IsLiveHere ? "是" : "否",
                                           pr.RelationWithHouseholder,
                                           pr.LodgingReason,
                                           pr.PopulationCharacter
                                       };

                var psdata = roomsWithPersons.ToList();

                var data = from pr in psdata
                           join sg in _context.SpecialGroups on pr.PersonId equals sg.PersonId into psg // 根据身份证关联
                           select new
                           {
                               pr.RoomId,
                               pr.RoomNO,
                               pr.CommunityName,
                               pr.SubdivsionName,
                               pr.BulidingName,
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
                return data;
            }
            catch (Exception e)
            {
                return null;
            }


        }

        #region 姓名身份证号电话搜索
        ///<summery>
        ///通过姓名身份证号电话搜索
        /// </summery>
        public IEnumerable<object> GetPersonsBySearch(string SubdivisionId, string Name)
        {
            try
            {
                //根据 room - person 数据
                var roomsWithPersons = from person in _context.Persons.Where(r => r.Name.Contains(Name) || r.PersonId == Name || r.Phone == Name)
                                       from pr in person.PersonRooms
                                       select new
                                       {
                                           RoomId = pr.Room.Id,
                                           RoomNO = pr.Room.Name,
                                           BulidingName = pr.Room.Building.Name,
                                           SubdivsionName = pr.Room.Building.Subdivision.Name,
                                           SubdivisionId = pr.Room.Building.Subdivision.Id.ToString(),
                                           CommunityName = pr.Room.Building.Subdivision.Community.Name,
                                           // person.PersonId,
                                           pr.Person,
                                           IsOwner = pr.IsOwner ? "是" : "否",
                                           IsHouseholder = pr.IsHouseholder ? "是" : "否",
                                           IsLiveHere = pr.IsLiveHere ? "是" : "否",
                                           pr.RelationWithHouseholder,
                                           pr.LodgingReason,
                                           pr.PopulationCharacter
                                       };
                if (SubdivisionId != null && SubdivisionId != string.Empty)
                {
                    roomsWithPersons = roomsWithPersons.Where(item => item.SubdivisionId == SubdivisionId || item.SubdivsionName == SubdivisionId);       //.Contains(SubdivisionId));  //== SubdivisionId );

                }
                var psdata = roomsWithPersons.ToList();

                var data = from pr in psdata
                           join sg in _context.SpecialGroups on pr.Person.PersonId equals sg.PersonId into psg // 根据身份证关联
                           select new
                           {
                               pr.RoomId,
                               pr.RoomNO,
                               pr.Person.PersonId,
                               pr.CommunityName,
                               pr.SubdivsionName,
                               pr.BulidingName,
                               pr.Person,
                               pr.IsOwner,
                               pr.IsHouseholder,
                               pr.IsLiveHere,
                               pr.RelationWithHouseholder,
                               pr.LodgingReason,
                               pr.PopulationCharacter,
                               SpecialGroup = psg // 特殊人群信息
                           };
                return data;
            }
            catch (Exception e)
            {
                return null;
            }


        }
        #endregion

        /// <summary>
        /// 获取特殊人群
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetSpecialGroups()
        {
            try
            {
                //根据 room - person 数据
                var roomsWithPersons = from room in _context.Rooms
                                       from pr in room.PersonRooms
                                       select new
                                       {
                                           RoomId = room.Id,
                                           RoomNO = room.Name,
                                           BulidingName = room.Building.Name,
                                           SubdivsionName = room.Building.Subdivision.Name,
                                           CommunityName = room.Building.Subdivision.Community.Name,
                                           pr.PersonId,
                                           pr.Person,
                                           IsOwner = pr.IsOwner ? "是" : "否",
                                           IsHouseholder = pr.IsHouseholder ? "是" : "否",
                                           IsLiveHere = pr.IsLiveHere ? "是" : "否",
                                           pr.RelationWithHouseholder,
                                           pr.LodgingReason,
                                           pr.PopulationCharacter
                                       };
                var psdata = roomsWithPersons.ToList();

                var data = from pr in psdata
                           join sg in _context.SpecialGroups on pr.PersonId equals sg.PersonId //into psg // 根据身份证关联
                           //where pr.PersonId = sg.PersonId
                           select new
                           {
                               pr.RoomId,
                               pr.RoomNO,
                               pr.CommunityName,
                               pr.SubdivsionName,
                               pr.BulidingName,
                               pr.PersonId,
                               pr.Person,
                               pr.IsOwner,
                               pr.IsHouseholder,
                               pr.IsLiveHere,
                               pr.RelationWithHouseholder,
                               pr.LodgingReason,
                               pr.PopulationCharacter,
                               sg.Type,
                               //SpecialGroup = psg // 特殊人群信息

                           };
                return data;
            }
            catch (Exception e)
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

        #region 高级检索
        /// <summary>
        /// 获取所有字段
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetFields()
        {
            String[] fields = { "小区", "楼栋", "房间", "姓名", "电话", "身份证", "年龄", "民族" };
            return fields;
        }
      
        ///<summary>
        ///高级检索
        /// 
        /// </summary>
        //主入口函数
        public IEnumerable<object> GetDataByQuery(List<string[]> queries)
        {
            IQueryable<Room> rooms = (IQueryable<Room>)FilterRooms(queries);


            return GetPersonsByQueryRoom(rooms);

        }

        //第一步：前端传来的过滤条件
        private IEnumerable<object> FilterRooms (List<string[]> queries) 
        {
            var rooms = _context.Rooms.AsQueryable();
            foreach (var query in queries)
            {
                if (query[0] == "小区")
                {
                    rooms = rooms.Where(r => r.Building.Subdivision.Name.Contains(query[2]));
                }
                if (query[0] == "楼栋")
                {
                    rooms = rooms.Where(r => r.Building.Name.Contains(query[2]) || r.Building.Alias.Contains(query[2]));
                }
                if (query[0] == "房间")
                {
                    rooms = rooms.Where(r => r.Name.Contains(query[2]));
                }
                if (query[0] == "姓名")
                {
                    rooms = rooms.Where(r => r.PersonRooms.Any(pr => pr.Person.Name.Contains(query[2])));
                }
                if (query[0] == "电话")
                {
                    rooms = rooms.Where(r => r.PersonRooms.Any(pr => pr.Person.Phone == query[2]));
                }
                if (query[0] == "身份证")
                {
                    rooms = rooms.Where(r => r.PersonRooms.Any(pr => pr.Person.PersonId == query[2]));
                }
                if (query[0] == "年龄")
                {
                    try
                    {
                        string[] age = query[2].Split('-');
                        int start = int.Parse(age[0]);
                        int end = int.Parse(age[1]);

                        rooms = rooms
                            .Include(r => r.PersonRooms).ThenInclude(pr => pr.Person)
                            .Include(r => r.Building).ThenInclude(b => b.Subdivision).ThenInclude(s => s.Community)
                            .AsEnumerable()
                            .Where(r => r.PersonRooms.Any(pr => pr.Person.Age > start && pr.Person.Age < end)).AsQueryable();
                    }
                    catch (Exception e)
                    {

                    }

                }
            }
            return rooms;
        }

        //第二步：构造二维表rooms
        //private IEnumerable<object> TwoTable (IQueryable<Room> rooms)
        //{
        //    var roomsWithPersons = from room in rooms
        //                           from pr in room.PersonRooms
        //                           select new
        //                           {
        //                               RoomId = room.Id,
        //                               RoomNO = room.Name,
        //                               BulidingName = room.Building.Name,
        //                               SubdivsionName = room.Building.Subdivision.Name,
        //                               CommunityName = room.Building.Subdivision.Community.Name,
        //                               pr.PersonId,
        //                               pr.Person,
        //                               IsOwner = pr.IsOwner ? "是" : "否",
        //                               IsHouseholder = pr.IsHouseholder ? "是" : "否",
        //                               IsLiveHere = pr.IsLiveHere ? "是" : "否",
        //                               pr.RelationWithHouseholder,
        //                               pr.LodgingReason,
        //                               pr.PopulationCharacter
        //                           };       
        //}
        private IEnumerable<object> GetPersonsByQueryRoom(IQueryable<Room> rooms)
        {
            try
            {
                var roomsWithPersons = from room in rooms
                                       from pr in room.PersonRooms
                                       select new
                                       {
                                           RoomId = room.Id,
                                           RoomNO = room.Name,
                                           BulidingName = room.Building.Name,
                                           SubdivsionName = room.Building.Subdivision.Name,
                                           CommunityName = room.Building.Subdivision.Community.Name,
                                           pr.PersonId,
                                           pr.Person,
                                           IsOwner = pr.IsOwner ? "是" : "否",
                                           IsHouseholder = pr.IsHouseholder ? "是" : "否",
                                           IsLiveHere = pr.IsLiveHere ? "是" : "否",
                                           pr.RelationWithHouseholder,
                                           pr.LodgingReason,
                                           pr.PopulationCharacter
                                       };
                var psdata = roomsWithPersons.ToList();
                var data = from pr in psdata
                           join sg in _context.SpecialGroups on pr.PersonId equals sg.PersonId into psg // 根据身份证关联
                           select new
                           {
                               pr.RoomId,
                               pr.RoomNO,
                               pr.CommunityName,
                               pr.SubdivsionName,
                               pr.BulidingName,
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
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        #endregion

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
