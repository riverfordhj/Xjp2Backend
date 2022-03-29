using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelCompany
{
    public class OutsideCompany
    {
        [Key]
        public int Id { get; set; }
        //企业名称
        public string CompanyName { get; set; }
        [Required]
        //统一社会信用代码
        public string UnifiedSocialCreditCode { get; set; }
        //经度
        public double Long { get; set; }
        //纬度
        public double Lat { get; set; }
        //高程
        public double Height { get; set; }
        //是否在调查的楼宇内
        public string IsWhereBuild { get; set; }
        //企业实际地址
        public string ActualOfficeAddress { get; set; }
        //企业注册地址
        public string RegisteredAddress { get; set; }
        //注册时间
        public string RegisteredTime { get; set; }
        //注册资金
        public string RegisteredCapital { get; set; }
        //行业名称industry
        public string IndustryName { get; set; }
        //行业代码
        public string IndustryCode { get; set; }
        //企业主营产品、服务
        public string MainProducts { get; set; }
        //税收
        public double Tax { get; set; }
    }
}
