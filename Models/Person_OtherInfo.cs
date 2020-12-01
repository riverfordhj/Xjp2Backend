using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    /// <summary>
    /// 特殊人群
    /// </summary>
    public class SpecialGroup
    {
        [Key]
        public int Id { get; set; }
        //身份证
        public string PersonId { get; set; }
        //类别
        public string Type { get; set; }
        //备注
        public string Note { get; set; }
        public Person Person { get; set; }
    }

    /// <summary>
    /// 困难人群
    /// </summary>
    public class PoorPeople
    {
        [Key]
        public int Id { get; set; }
        //身份证
        public string PersonId { get; set; }
        //类别
        public string Type { get; set; }
        //困难儿童
        public string Child { get; set; }
        //重点青少年
        public string Youngsters { get; set; }
        //特扶
        public string SpecialHelp { get; set; }
        //备注
        public string Note { get; set; }
    }


    ///服役状况        
    public class MilitaryService
    {
        [Key]
        public int Id { get; set; }
        //身份证
        public string PersonId { get; set; }
        //类别
        public string Type { get; set; }
        //备注
        public string Note { get; set; }
    }



    ///残疾        
    public class Disability
    {
        [Key]
        public int Id { get; set; }
        //身份证
        public string PersonId { get; set; }
        //残疾类别
        public string Type { get; set; }
        //残疾等级
        public string Class { get; set; }
        //备注
        public string Note { get; set; }
    }

    ///其他信息    
    public class OtherInfos
    {
        [Key]
        public int Id { get; set; }
        //身份证
        public string PersonId { get; set; }
       //
        public string Key { get; set; }
        //
        public string Value { get; set; }
        //备注
        public string Note { get; set; }
    }
}
