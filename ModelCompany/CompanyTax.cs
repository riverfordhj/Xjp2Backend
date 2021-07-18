using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ModelCompany
{
    public class CompanyTax 
    {
        [Key]
        public int Id { get; set; }
        //年份
        public int Year { get; set; }
        //营收
        public double Revenue { get; set; }
        //税收
        public double Tax { get; set; }
        //税收所在地
        public string TaxAdress { get; set; }

        public CompanyBasicInfo CompanyBasicInfo { get; set; }

    }
}
