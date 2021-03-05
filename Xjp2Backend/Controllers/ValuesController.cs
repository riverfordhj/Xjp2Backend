using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Xjp2Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("[action]")]
        public ActionResult<IEnumerable<string>> GetTestValue()
        {
            return new string[] { "TestValue" };
        }
        [HttpGet("[action]")]
        [Authorize(Policy = "APIAccess")]
        public ActionResult<IEnumerable<string>> GetValueByGuestPolicy()
        {
            return new string[] { "use policy = APIAccess" };
        }

        [HttpGet("[action]")]
        [Authorize(Policy = "Admin")]
        public ActionResult<IEnumerable<string>> GetValueByAdminPolicy()
        {
            return new string[] { "use policy = Administrator" };
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Administrator")]
        public ActionResult<IEnumerable<string>> GetValueByAdminRole()
        {
            return new string[] { "use Roles = Administrator" };
        }

        [HttpGet("[action]")]
        [Authorize(Policy = "Permission")]
        public ActionResult<IEnumerable<string>> GetAdminValue()
        {
            return new string[] { "use Policy = Permission" };
        }

        [HttpGet("[action]")]
        [Authorize(Policy = "Permission")]
        public ActionResult<IEnumerable<string>> GetGuestValue()
        {
            return new string[] { "use Policy = Permission" };
        }

        [HttpGet("[action]")]
        [Authorize(Policy = "SAXC_Grid")]
        public ActionResult<IEnumerable<string>> GetValueBySAXC_Grid()
        {
            return new string[] { "use Policy = saxc_Grid" };
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "网格员")]
        public ActionResult<IEnumerable<string>> GetValueBySAXC_Grid_Roles()
        {
            return new string[] { "use Roles = 网格员" };
        }

        [HttpGet("[action]")]
        [Authorize(Policy = "SAXC")]
        public ActionResult<IEnumerable<string>> GetValueBySAXC()
        {
            return new string[] { "use Policy = saxc" };
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "水岸星城社区")]
        public ActionResult<IEnumerable<string>> GetValueBySAXC_Roles()
        {
            return new string[] { "use Roles = 水岸星城社区" };
        }
    }
}
