using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ModelCompany
{
    public class CompanyOtherInfo
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int Year { get; set; }
        //员工人数
        public int EmployeesNum { get; set; }
        //本科及以上学历员工数bachelor degree or above
        public int BachelorAboveNum { get; set; }
        //中高级职称员工数mediate and Senior
        public int MiddleAndSeniorNum { get; set; }
        //党员人数party member
        public int PartyMember { get; set; }
        //办公面积
        public double OfficeArea { get; set; }
        //办公租金（月）rental
        public double OfficeRental { get; set; }
        //企业专利数
        public int PatentNum { get; set; }
        public CompanyBasicInfo CompanyBasicInfo { get; set; }

    }
}
