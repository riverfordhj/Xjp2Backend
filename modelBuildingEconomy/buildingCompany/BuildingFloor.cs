using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelsBuildingEconomy.buildingCompany
{
    public class BuildingFloor
    {
        [Key]
        public int Id { get; set; }
        public string Community { get; set; }
        public string BuildingName { get; set; }
        public string FloorNum { get; set; }
        public double Long { get; set; }
        public double Lat { get; set; }
        public decimal Height { get; set; }

        public CompanyBuilding CompanyBuilding{ get;set;}

    }
}
