using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


/// <summary>
/// WebAPI 后端接受参数帮助类
/// </summary>
namespace Xjp2Backend.Controllers
{
    /// <summary>
    /// 接受前段登录参数
    /// </summary>
    public class LoginRequest
    {
        [Required(ErrorMessage = "{0} is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(128, MinimumLength = 6, ErrorMessage = "minimum Length of {0} is 6.")]
        public string Password { get; set; }
    }

    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "{0} is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string RefreshToken { get; set; }
    }

    public class PersonInRoomParameter
    {
        [Required(ErrorMessage = "{0} 不能为空！")]
        public string SubdivisionName { get; set; }

        [Required(ErrorMessage = "{0} 不能为空！")]
        public string BuildingName { get; set; }

        [Required(ErrorMessage = "{0} 不能为空！")]
        public string RoomNO { get; set; }
    }

    public class CompanyFieldsParameter
    {
        [Required(ErrorMessage ="{0} 不能为空! ")]
        public string BuildingName { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string Contacts { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string BusinessDirection { get; set; }
    }

    public class BuildingFloor
    {
        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string BuildingName { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string Floor { get; set; }

    }


    public class BuildingStatus
    {
        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string BuildingName { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string status { get; set; }
    }

}
