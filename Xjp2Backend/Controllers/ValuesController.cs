using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Xjp2Backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]//身份验证通过才能访问
    [ApiController]
    public class ValuesController : ControllerBase
    {
        //同属于策略“Admin”的成员才能访问
        [HttpGet("[action]")]
        [Authorize(Policy = "Admin")]
        public ActionResult<IEnumerable<string>> GetValueByAdminPolicy()
        {
            return new string[] { "use policy = Admin" };
        }
        //同属于角色“Administrator”的成员才能访问
        [HttpGet("[action]")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<IEnumerable<string>> GetValueByAdminRole()
        {
            return new string[] { "use Roles = Administrator" };
        }
        //同属于策略“Grider”的成员才能访问
        [HttpGet("[action]")]
        [Authorize(Policy = "Grider")]
        public ActionResult<IEnumerable<string>> GetValueByGrider()
        {
            return new string[] { "use Policy = Grider" };
        }
        //同属于角色“网格员”的成员才能访问
        [HttpGet("[action]")]
        [Authorize(Roles = "网格员")]
        public ActionResult<IEnumerable<string>> GetValueByGriderRole()
        {
            return new string[] { "use Roles = 网格员" };
        }
        //同属于策略“Audit”的成员才能访问
        [HttpGet("[action]")]
        [Authorize(Policy = "Community")]
        public ActionResult<IEnumerable<string>> GetValueByCommunity()
        {
            return new string[] { "use Policy = Community" };
        }
        //同属于角色“社区”的成员才能访问
        [HttpGet("[action]")]
        [Authorize(Roles = "社区")]
        public ActionResult<IEnumerable<string>> GetValueByCommunityRoles()
        {
            return new string[] { "use Roles = 社区" };
        }
        //同属于策略“Audit”的成员才能访问
        [HttpGet("[action]")]
        [Authorize(Policy = "Audit")]
        public ActionResult<IEnumerable<string>> GetValueByAudit()
        {
            return new string[] { "use Policy = Audit" };
        }
    }
}
