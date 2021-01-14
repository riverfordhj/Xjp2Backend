using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [Required]
        //身份证
        public string PersonId { get; set; }
        [Required]
        //姓名   
        public string Name { get; set; }
        //民族
        public string EthnicGroups { get; set; }
        //电话
        public string Phone { get; set; }
        //户籍地址
        public string DomicileAddress { get; set; }
        //从业单位
        public string Company { get; set; }
        //政治面貌
        public string PoliticalState { get; set; }
        //组织关系
        public string OrganizationalRelation { get; set; }
        //是否侨胞
        public bool IsOverseasChinese { get; set; }
        //婚姻状况
        public string MerriedStatus { get; set; }

        //备注
        public string Note { get; set; }
        [NotMapped]
        public int Age
        {
            get
            {
                try
                {
                    int year = DateTime.Now.Year;
                    int age = year - int.Parse(PersonId.Substring(6,4));
                    return age;
                }
                catch(Exception e)
                {
                    return 0;
                }
            }
        }
        [NotMapped]
        public string Sex
        {
            get
            {
                try
                {
                    string sex;
                    if ( int.Parse(PersonId.Substring(16, 1)) % 2 ==0)
                    {
                        sex = "女";
                    }
                    else
                    {
                        sex = "男";
                    }
                    return sex;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        //导航属性
        public List<PersonRoom> PersonRooms { get; set; }
    }

    
}
