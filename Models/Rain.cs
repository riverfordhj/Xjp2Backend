using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class Rain
    {
        [Key]
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
        //状态
        public string Status { get; set; }
        //类型
        public string Type { get; set; }
        //地址
        public string Address { get; set; }
        //备注
        public string Note { get; set; }
    }
}