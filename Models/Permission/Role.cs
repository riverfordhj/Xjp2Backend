using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //备注
        public string Note { get; set; }

        //导航属性
        public List<RoleUser> RoleUsers { get; set; }

        [NotMapped]
        public List<User> Users
        {
            get
            {
                if (RoleUsers != null)
                    return RoleUsers.Select(item => item.User).ToList();
                else
                    return null;
            }
        }
    }
}
