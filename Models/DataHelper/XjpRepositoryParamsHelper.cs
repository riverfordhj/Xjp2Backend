using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DataHelper
{
    class XjpRepositoryParamsHelper
    {
    }

    public class PersonUpdateParamTesting
    {
        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string PersonId { get; set; }
        public string Name { get; set; }
        public string EthnicGroups { get; set; }
        public string Phone { get; set; }
        public string DomicileAddress { get; set; }
        public string Company { get; set; }
        public string PoliticalState { get; set; }
        public string OrganizationalRelation { get; set; }
        public string IsOverseasChinese { get; set; }
        public string MerriedStatus { get; set; }
        public string Note { get; set; }

        public string IsHouseholder { get; set; }
        public string RelationWithHouseholder { get; set; }
        public string IsOwner { get; set; }
        public string IsLiveHere { get; set; }
        public string PopulationCharacter { get; set; }
        public string LodgingReason { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string RoomName { get; set; }

        public string RoomUse { get; set; }
        public string Area { get; set; }

        public string Category { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string BuildingName { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string Address { get; set; }
        public string SubdivisionName { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string NetGrid { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string CommunityName { get; set; }
        public string StreetName { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string Operation { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string Status { get; set; }
    }

    public class VerifyAndConfirmParam
    {
        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string PersonId { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string RoomName { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string BuildingName { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string NetGrid { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string CommunityName { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string Status { get; set; }
    }

    public class RoomCreatingParam
    {
        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string CommunityName { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string NetGridName { get; set; }
    
        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string BuildingName { get; set; }


        [Required(ErrorMessage = "{0} 不能为空! ")]
        public int Units { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public int Floors { get; set; }
        public string Address { get; set; }

    }

    public class RoomCreatingParam_Other 
    { 
        public int id { get; set; }
        public string CommunityName { get; set; }
        public string NetGridName { get; set; }
        public string BuildingName { get; set; }
        public string Address { get; set; }
        public string RoomName { get; set; }
        public string Category { get; set; }
        public string Use { get; set; }
        public string Area { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Height { get; set; }
    }

    public class UpdatePasswordParam
    {
        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string Account { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string LastPassword { get; set; }

        [Required(ErrorMessage = "{0} 不能为空! ")]
        public string Password { get; set; }
        public string CheckPassword { get; set; }
    }

    public class ResetUserPasswordParam 
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password{ get; set; }
        public string phone { get; set; }
    }

    public class SavepathRowParam
    {
        public int Id { get; set; }
        //名称
        public string Name { get; set; }
        //经度
        public double Longitude { get; set; }
        //纬度
        public double Latitude { get; set; }
        //高程
        public double Height { get; set; }
        //上报人
        public string Report { get; set; }
        //上报时间
        public string ReportTime { get; set; }
        //状态
        public string Status { get; set; }
        //类型
        public string Type { get; set; }
        //地址
        public string Address { get; set; }
        //文件名称
        public string FileName { get; set; }
        //文件路径
        public string FilePath { get; set; }
        //备注
        public string Note { get; set; }
    }

    public class RainUpdateParamTesting
    {
        public int id { get; set; }
        public string Name { get; set; }
        //经度
        [Required(ErrorMessage = "{0} 不能为空! ")]
        public double Longitude { get; set; }
        //纬度
        [Required(ErrorMessage = "{0} 不能为空! ")]
        public double Latitude { get; set; }
        //高程
        [Required(ErrorMessage = "{0} 不能为空! ")]
        public double Height { get; set; }
        //上报人
        public string Report { get; set; }
        //状态
        public string Status { get; set; }
        public string ReportTime { get; set; }
        //类型
        public string Type { get; set; }
        //地址
        public string Address { get; set; }
        //备注
        public string Note { get; set; }
    }
}
