using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelsBuildingEconomy.buildingCompany
{
    public class CompanyTaxInfo
    {
        [Key]
        public int Id { get; set; }
        public string UnifiedSocialCreditCode { get; set; }
        public string TaxPayer { get; set; }
        public int TaxYear { get; set; }
        public double TotalTax { get; set; }
        public double BusinessTax { get; set; }
        public double ValueAddedTax { get; set; }
        public double CorporateIncomeTax { get; set; }
        public double IndividualIncomeTax { get; set; }
        public double UrbanConstructionTax { get; set; }
        public double RealEstateTax { get; set; }
        public double StampDuty { get; set; }
        public double LandUseTax { get; set; }
        public double LandValueIncrementTax { get; set; }
        public double VehicleAndVesselTax { get; set; }
        public double DeedTax { get; set; }
        public double AdditionalTaxOfEducation { get; set; }
        public double DelayedTaxPayment { get; set; }
        public string RegisteredAddress { get; set; }

    }
}
