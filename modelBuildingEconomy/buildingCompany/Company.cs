using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelsBuildingEconomy.buildingCompany
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string UnifiedSocialCreditCode { get; set; }
        public string RegisteredAddress { get; set; }
        public string ActualOfficeAddress { get; set; }
        public string RegisteredCapital { get; set; }
        public string IsIndependentLegalEntity { get; set; }
        public string LegalRepresentative { get; set; }
        public string Contacts { get; set; }
        public string Phone { get; set; }
        public string EnterpriseType { get; set; }
        public string EnterpriseBackground { get; set; }
        public string BusinessDirection { get; set; }
        public string RegistrationPlace { get; set; }
        public string TaxStatisticsArea { get; set; }
        public string note { get; set; }


        public CompanyBuilding CompanyBuilding { get; set; }
        public CompanyEconomy CompanyEconomy { get; set; }
        public CompanyOtherInfo CompanyOtherInfo { get; set; } 
        public CompanyTaxInfo CompanyTaxInfo { get; set; }

      
    }
}
