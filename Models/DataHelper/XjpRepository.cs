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

        public IEnumerable<StreetUnit> GetStreets()
        {
            return _context.Streets.ToList();//.Take(10)
        }

        public bool StreetExists(int id)
        {
            return _context.Streets.Any(item => item.Id == id);
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
