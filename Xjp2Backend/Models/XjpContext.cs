using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xjp2Backend.Models
{
    public class XjpContext : DbContext
    {
        public XjpContext()
            : base()
        {
        }
        public XjpContext(DbContextOptions<XjpContext> options)
            : base(options)
        {
        }

        public DbSet<PartyInfo> PartyInfos { get; set; }
    }
}
