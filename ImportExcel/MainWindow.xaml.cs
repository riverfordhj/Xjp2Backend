using Microsoft.Win32;
using Models;
using Models.DataHelper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using Xjp2Backend.Models;
//更改作者

namespace ImportExcel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void bn_Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Excel文件(*.xlsx)|*.xlsx|所有文件(*.*)|*.*";

                if (ofd.ShowDialog() == true)
                {
                    tbPath.Text = ofd.FileName;

                }
            }
            catch (Exception err)
            {
                tbInfo.Text = err.Message;
            }


        }

        private void bn_Add_Click(object sender, RoutedEventArgs e)
        {           
            try
            {
                tbInfo.Text = ReadExcelData(tbPath.Text);

            }
            catch (Exception err)
            {
                tbInfo.Text = $"{err.Message}{ Environment.NewLine} {_i + 4}, {_currentLine}";
                //tbInfo.Text = err.Message;
            }
            //using (var db = new ImportContext())
            //{
            //    var blog = new PartyInfo { Name = "name1", Note = "node1" };
            //    db.PartyInfos.Add(blog);
            //    db.SaveChanges();
            //}

        }

        private string ReadExcelData(string path)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            FileInfo fi = new FileInfo(path);
            StringBuilder sb = new StringBuilder();
            List<string> data = new List<string>();
            using (ExcelPackage package = new ExcelPackage(fi))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int ColCount = worksheet.Dimension.Columns;

                for (int row = 4; row <= rowCount; row++)//在excel表中第一行是标题，所以数据是从第二行开始的。
                {
                    StringBuilder sbRow = new StringBuilder();
                    for (int col = 1; col <= ColCount; col++)
                    {
                        if (worksheet.Cells[row, col].Value == null)
                            sbRow.Append(",");
                        else
                            sbRow.Append(worksheet.Cells[row, col].Value.ToString() + ",");
                    }
                    data.Add(sbRow.ToString());
                    sb.Append(sbRow.ToString() + Environment.NewLine);
                }
            }

            Add2DB(data);

            return sb.ToString();
        }

        string _currentLine = "";
        string _errorMessage = "";
        int _i = 0;
        string[] _preItem = null;
        private void Add2DB(List<string> data)
        {
            //if (data.Count > 0)
            for (_i = 0; _i < data.Count; _i++)
            {
                _currentLine = data[_i];
                string[] item = _currentLine.Split(',');
                //string[] item = data[0].Split(',');
                if (IsEmpty(item, 6))
                    continue;

                CheckAllValue(item, 6);

                using (var context = new StreetContext())
                {
                    //街道

                    StreetUnit street = context.Streets.SingleOrDefault(s => s.Name == "徐家棚");

                    if (street == null)
                    {
                        street = new StreetUnit { Name = "徐家棚" };
                        context.Streets.Add(street);
                    }

                    //社区
                    var community = context.Communitys.SingleOrDefault(s => s.Name == item[0].Replace("社区",""));
                    if (community == null)
                    {
                        //CheckValue(item,0);
                        community = new Community { Name = item[0].Replace("社区","") };
                        community.Street = street;
                        //street.Communities.Add(community);
                        context.Communitys.Add(community);
                    }

                    //网格
                    var netGrid = context.NetGrids.SingleOrDefault(s => s.Community.Id == community.Id && s.Name == item[1]);
                    if (netGrid == null)
                    {
                       // CheckValue(item, 1);
                        netGrid = new NetGrid { Name = item[1] };
                        netGrid.Community = community;
                        context.NetGrids.Add(netGrid);
                    }


                    //小区
                    var subdivision = context.Subdivisions.SingleOrDefault(s => s.Name == item[3].Replace("小区",""));
                    if (subdivision == null)
                    {
                        //CheckValue(item,3);
                        subdivision = new Subdivision { Name = item[3].Replace("小区","") };
                        subdivision.Street = street;
                        context.Subdivisions.Add(subdivision);
                    }

                    //楼栋

                    //var building = netGrid.Buildings.SingleOrDefault(s => s.Name == item[4]);
                    var building = context.Buildings.SingleOrDefault(s => s.Subdivision.Id == subdivision.Id && s.Name == item[4].Replace("栋",""));
                    if (building == null)
                    {
                        building = new Building { Name = item[4].Replace("栋","") };
                        building.NetGrid = netGrid;
                        building.Subdivision = subdivision;
                        context.Buildings.Add(building);
                        //subdivision.Buildings.Add(building);
                    }

                    //房屋
                    string roomName = $"{item[5].Replace("单元","")}-{item[6].Replace("号","")}";
                    var room = context.Rooms.SingleOrDefault(r => r.Building.Id == building.Id && r.Name == roomName);
                    if (room == null)
                    {
                        room = new Room 
                        { 
                            Name = roomName ,
                            Address = item[2],
                            Category = item[7],
                            Use = item[8],
                            Area = item[10],
                            Other = item[9],

                        };
                        room.Building = building;
                        context.Rooms.Add(room);
                    }

                    //单位信息
                  //  var companyinfo = new CompanyInfo { };
                    if (item[11] != "")
                    {
                        var companyinfo = new CompanyInfo
                        {
                            Name = item[11],
                            Character = item[12],
                            SocialId = item[13],
                            ContactPerson = item[14],
                            PersonId = item[15],
                            Phone = item[16],
                            Area = item[17]
                        };
                         context.CompanyInfos.Add(companyinfo);
                    }
                   


                    //人
                    var person = context.Persons.SingleOrDefault(p => p.PersonId == item[20]);

                    //检测空名空身份证号
                    if(!CheckItem(item))
                        continue;
                    //检测同身份证号不同名
                    if (!CheckPerson(person, item))
                        continue;

                    if (person == null)
                    {                     
                        person = new Person
                        {
                            Name = item[18],
                            EthnicGroups = item[19],
                            PersonId = item[20],
                            Phone = item[21],
                            DomicileAddress = item[22],

                            Company = item[27],
                            PoliticalState = item[28],
                            OrganizationalRelation = item[29],
                            IsOverseasChinese = (item[30] == "是"),
                            MerriedStatus = item[31],
                        };
                       // person.CompanyInfo = companyinfo;
                        context.Persons.Add(person);


                        //单位信息
                        //  var companyinfo = new CompanyInfo { };
                        if (item[11] != "")
                        {
                            var companyinfo = new CompanyInfo
                            {
                                Name = item[11],
                                Character = item[12],
                                SocialId = item[13],
                                ContactPerson = item[14],
                                PersonId = item[15],
                                Phone = item[16],
                                Area = item[17]
                            };
                            context.CompanyInfos.Add(companyinfo);
                        }
                        //特殊人群

                        if (item[34] != "")
                        {
                            //var specialGroup = context.SpecialGroups.SingleOrDefault(s => s.PersonId == item[20]);
                            //if (specialGroup == null)                           
                            var specialGroup = new SpecialGroup { PersonId = item[20], Type = item[34] };
                            context.SpecialGroups.Add(specialGroup);
                        }

                        // 困难人群
                        if (item[35] != "")
                        {
                            var poorPeople = new PoorPeople
                            {
                                PersonId = item[20],
                                Type = item[35],
                                Child = item[36],
                                Youngsters = item[37],
                                SpecialHelp = item[38]
                            };
                            context.PoorPeoples.Add(poorPeople);
                        }
                        //服役状况
                        if (item[39] != "")
                        {
                            var militaryService = new MilitaryService { PersonId = item[20], Type = item[39] };
                            context.MilitaryService.Add(militaryService);
                        }

                        //残疾   
                        //var disability = context.Disabilitys.SingleOrDefault(s => s.PersonId == item[20]);
                        //if (disability == null)
                        if (item[40] != "")
                        {
                            var disability = new Disability
                            {
                                PersonId = item[20],
                                Type = item[40],
                                Class = item[41],
                            };
                            context.Disability.Add(disability);
                        }

                        //其他信息 
                        //var otherInfos = context.OtherInfos.SingleOrDefault(s => s.PersonId == item[20]);
                        // if (otherInfos == null)
                        if (item[42] != "")
                        {
                            var otherInfos = new OtherInfos
                            {
                                PersonId = item[20],
                                //Key = item[42],
                                Value = item[42],
                            };
                            context.OtherInfos.Add(otherInfos);
                        }
                    }
                    //personroom 人房信息
                    var personHouse = new PersonRoom
                    {
                        PersonId = item[20],
                        IsHouseholder = (item[23] == "是"),
                        RelationWithHouseholder = item[24],
                        IsOwner = (item[25] == "是"),
                        IsLiveHere = (item[26] == "是"),
                        PopulationCharacter = item[32],
                        LodgingReason = item[33]
                    };

                    personHouse.Person = person;
                    personHouse.Room = room;
                    context.PersonRooms.Add(personHouse);



                    context.SaveChanges();
                    _preItem = item;
                }
            }
           // tbInfo_err.Text = "";
            tbInfo_err.Text += _errorMessage;
        }

        private void CheckAllValue(string[] items, int count)
        {
            if (items.Length < count)
                count = items.Length;

            for (int i = 0; i < count; i++)
            {
                string value = items[i].Trim();
                if (value == "")
                {
                    items[i] = _preItem[i];
                }
                   
            }

        }

        private bool IsEmpty(string[] item,int v)
        {
            foreach (var i in item)
            {
                string value = i.Trim();
                if (value != "")
                    return false;

            }
            return true;
        }

        //private void CheckValue(string[] item, int v)
        //{
        //    string value = item[v].Trim();
        //    if (value == "")
        //    {
        //        item[v] = _preItem[v];
        //    }
              
        //}

        #region check data
        //网格数据检测，空名空身份证号，同身份证号不同名
        private bool CheckItem(string[] item)
        {
            if (item[18] == "" || item[20] == "")
            {
                _errorMessage += _i + 4 + "姓名或身份证不能为空" + Environment.NewLine;
                return false;
            }
            return true;
        }
        private bool CheckPerson(Person person, string[] item)
        {

            if (person != null && person.Name != item[18])
            {
                _errorMessage += _i + 4 + "身份证重复" + Environment.NewLine;
                return false;
            }
            return true;
        }
        #endregion

        private void bnAddInitData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //string streetName = tbStreet.Text;
                string communiteName = tbCommuniteName.Text;
                string gridUser = tbGirdUser.Text;
                int count = int.Parse(tbGridCount.Text);

                string message = InitDataHelper.AddData(communiteName, gridUser,count );
                tbInfo.Text = message;
            }
            catch (Exception err)
            {
                tbInfo_err.Text += err.Message;
            }
        }
    }
}
