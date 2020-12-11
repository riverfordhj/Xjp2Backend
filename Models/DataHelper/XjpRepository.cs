using Microsoft.EntityFrameworkCore;
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

        //获取建筑物的住户信息(中文)
        public IEnumerable<object> GetPersonsByBuilding_ZH(int id)
        {            
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

                var psdata = roomsWithPersons.ToList();

                var data = from pr in psdata
                           join sg in _context.SpecialGroups on pr.PersonId equals sg.PersonId into psg // 根据身份证关联
                           select new
                           {
                               房间号 = pr.RoomNO,
                               社区名 = pr.CommunityName,
                               小区名 = pr.SubdivsionName,
                               楼栋名 = pr.BulidingName,
                               产权人 = pr.IsOwner,
                               户主 = pr.IsHouseholder,
                               在此居住 = pr.IsLiveHere,
                               与户主关系 = pr.RelationWithHouseholder,
                               寄住原因 = pr.LodgingReason,
                               人口性质 = pr.PopulationCharacter,
                           };

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
                    roomsWithPersons = roomsWithPersons.Where(item => item.SubdivisionId == SubdivisionId);       //.Contains(SubdivisionId));  //== SubdivisionId );

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
        
        //根据user，返回对应的小区
        public IQueryable<object> GetSubdivsionsByUser(string userName)
        {
            //var list =  _context.Roles.Where(r => r.Users.Any(u => u.UserName == userName)).Select(u => u);

            //var list = from userRow in _context.Users.Where(u => u.UserName == userName)
            //           from u_RoleUsers in userRow.RoleUsers
            //           join ru in _context.RoleUsers
            //           on u_RoleUsers.Id equals ru.Id
            //           select new
            //           {
            //               u_RoleUsers.Role.Name
            //           };
            //var roleList = list.ToList();

            //var isNetGrid = roleList.Find(role => role.Name == "网格员");

            var user = GetUserByName(userName);
            var roleList = user.Roles;



            if(roleList[0].Name == "网格员")
            {
                return (from useRow in _context.Users.Where(u => u.UserName == userName)
                        from grid in useRow.NetGrid
                        select new
                        {
                            grid.Community.Subdivisions[0].Id,
                            grid.Community.Subdivisions[0].Name
                        });
            
            }

            if (roleList[0].Name == "水岸星城")
            {
                return (from comm in _context.Communitys.Where(u => u.Name == "水岸星城")
                        select new
                        {
                            comm.Subdivisions[0].Id,
                            comm.Subdivisions[0].Name

                        });
            }

            //if (roleList[0].Name == "Administrator" && roleList[1].Name == "网格员")
            //{
            //    return _context.Subdivisions;
            //}

            return _context.Subdivisions;
        }

        public IQueryable<object> GetRoomsByUser(string userName)
        {

            var user = GetUserByName(userName);
            var roleList = user.Roles;

            if (roleList[0].Name == "网格员"){
                var roomData=  from u in _context.Users.Where(u => u.UserName == userName)
                               from ng in u.NetGrid
                               from b in ng.Buildings
                               from room in b.Rooms
                               select new
                               {
                                   room.Id,
                                   room.Name,
                                   room.Alias,
                                   room.Category,
                                   room.Use,
                                   room.Area,
                                   room.Longitude,
                                   room.Latitude,
                                   room.Height,
                                   room.Other,
                                   room.Note,
                                   buildingName = b.Name,
                                   netGridName = ng.Name,
                                   communityName = ng.Community.Name
                               };
                return roomData;
            }
            if (roleList[0].Name == "水岸星城")
            {
                var roomData = from comm in _context.Communitys.Where(c => c.Name == "水岸星城")
                               from ng in comm.NetGrids
                               from b in ng.Buildings
                               from room in b.Rooms
                               select new
                               {
                                   room.Id,
                                   room.Name,
                                   room.Alias,
                                   room.Category,
                                   room.Use,
                                   room.Area,
                                   room.Longitude,
                                   room.Latitude,
                                   room.Height,
                                   room.Other,
                                   room.Note,
                                   buildingName = b.Name,
                                   netGridName = ng.Name,
                                   communityName = comm.Name
                               };

                return roomData;
            }

            return from room in _context.Rooms
                   select new {
                       room.Id,
                       room.Name,
                       room.Alias,
                       room.Category,
                       room.Use,
                       room.Area,
                       room.Longitude,
                       room.Latitude,
                       room.Height,
                       room.Other,
                       room.Note,
                       buildingName = room.Building.Name,
                       netGridName = room.Building.NetGrid.Name,
                       communityName = room.Building.NetGrid.Community.Name
                   };

        }

        public IQueryable<object> GetPersonsByUser(string userName){
            var user = GetUserByName(userName);
            var roleList = user.Roles;

            if (roleList[0].Name == "网格员")
            {
                var personInfo = from u in _context.Users.Where(u => u.UserName == userName)
                                 from ng in u.NetGrid
                                 from b in ng.Buildings
                                 from room in b.Rooms
                                 from pr in room.PersonRooms
                                 select new
                                 {
                                    pr.Person,
                                    roomName = room.Name,
                                    roomUse = room.Use,
                                    roomCategory = room.Category,
                                    buildingName = b.Name,
                                    netGrid = ng.Name,
                                    communityName = ng.Community.Name
                                 };
                return personInfo;

            }

            if (roleList[0].Name == "水岸星城")
            {
                var personInfoByComm = from comm in _context.Communitys.Where(c => c.Name == "水岸星城")
                                 from ng in comm.NetGrids
                                 from b in ng.Buildings
                                 from room in b.Rooms
                                 from pr in room.PersonRooms
                                 select new
                                 {
                                     pr.Person,
                                     roomName = room.Name,
                                     roomUse = room.Use,
                                     roomCategory = room.Category,
                                     buildingName = b.Name,
                                     netGrid = ng.Name,
                                     communityName = ng.Community.Name
                                 };


                var personInfoByStatus = from p in _context.Persons.Where(per => per.Status != null && per.Status != "commiting")
                                         select p;

                var result = from pc in personInfoByComm
                             join ps in personInfoByStatus on pc.Person.PersonId equals ps.PersonId
                             select pc;
                        
                                        
                return result;

            }

            //不满足以上条件时，筛选全部数据
            var personInfoByAdmin = from comm in _context.Communitys
                                    from ng in comm.NetGrids
                                    from b in ng.Buildings
                                    from room in b.Rooms
                                    from pr in room.PersonRooms
                                    select new
                                    {
                                        pr.Person,
                                        roomName = room.Name,
                                        roomUse = room.Use,
                                        roomCategory = room.Category,
                                        buildingName = b.Name,
                                        netGrid = ng.Name,
                                        communityName = ng.Community.Name
                                    };
            var PersonInfoForCommiting = from p in _context.Persons.Where(per => per.Status == "commiting")
                                         select p;

            return from pc in personInfoByAdmin
                   join ps in PersonInfoForCommiting on pc.Person.PersonId equals ps.PersonId
                   select pc;
        }

        //网格员修改指定人员信息
        public IQueryable<object> UpdatePersonHouseByNetGrid(string userName, string personId, string phoneNum, string status)
        {
            Person person = _context.Persons.FirstOrDefault(p => p.PersonId == personId);
            person.EditingPhone = phoneNum;
            person.Status = status;
            _context.SaveChanges();

            return from u in _context.Users.Where(u => u.UserName == userName)
                   from ng in u.NetGrid
                   from b in ng.Buildings
                   from room in b.Rooms
                   from pr in room.PersonRooms
                   select new
                   {
                       pr.Person,
                       roomName = room.Name,
                       roomUse = room.Use,
                       roomCategory = room.Category,
                       buildingName = b.Name,
                       netGrid = ng.Name,
                       communityName = ng.Community.Name
                   };
                        
        }

        public void ReviewByCommunity(string personId, string status)
        {
            Person person = _context.Persons.FirstOrDefault(p => p.PersonId == personId);
            person.Status = status;
            _context.SaveChanges();
        }

        public IQueryable<object> ConfirmByAdmin(string userName, string personId)
        {
            Person person = _context.Persons.FirstOrDefault(p => p.PersonId == personId);
            person.Status = null;
            person.Phone = person.EditingPhone;
            person.EditingPhone = null;
            _context.SaveChanges();

            return GetPersonsByUser(userName);
        }

        #endregion

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        
    }
}
