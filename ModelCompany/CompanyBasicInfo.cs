using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ModelCompany
{
    public class CompanyBasicInfo
    {
        [Key]
        public int Id { get; set; }
        //企业名称
        public string CompanyName { get; set; }
        [Required]
        //统一社会信用代码
        public string UnifiedSocialCreditCode { get; set; }
        //企业实际地址
        public string ActualOfficeAddress { get; set; }
        //企业注册地址
        public string RegisteredAddress { get; set; }
        //楼层
        public string FloorNum { get; set; }
        //企业占据房间号
        public string WorkRoomName { get; set; }
        //企业性质
        public string EnterpriseType { get; set; }
        //法定代表人
        public string LegalRepresentative { get; set; }
        //注册时间
        public string RegisteredTime { get; set; }
        //注册资金
        public string RegisteredCapital { get; set; }
        //固定电话
        public string FixedTelephone { get; set; }
        //联系人
        public string Contacts { get; set; }
        //职务
        public string Duty { get; set; }
        //邮箱
        public string Email { get; set; }
        //联系人电话
        public string Phone { get; set; }
        //公司网址 company web
        public string CompanyWeb { get; set; }
        //办公用房类型 Office space type
        public string OfficeSpaceType { get; set; }
        //是否建立党组织
        public string IsPartyPrganization { get; set; }
        //行业名称industry
        public string IndustryName { get; set; }
        //行业代码
        public string IndustryCode { get; set; }
        //是否享受过区级政策
        public string IsDistrictPolicy { get; set; }
        //企业主营产品、服务
        public string MainProducts { get; set; }
        //企业专利及科技成果
        public string PatentsandTechnological { get; set; }
        //应用范围（成功使用主要案例application
        public string Application { get; set; }
        //企业需求及建议 Suggestions
        public string Suggestions { get; set; }
        public string note { get; set; }


        public CompanyBuildings CompanyBuildings { get; set; }
        public List<CompanyRoom> CompanyRoom { get; set; }
        public List<CompanyOtherInfo> CompanyOtherInfo { get; set; }
        public List<CompanyTax> CompanyTax { get; set; }
    }
}