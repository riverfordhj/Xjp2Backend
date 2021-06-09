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
                List<string> data = ReadExcelData(tbPath.Text, 4);
                // tbInfo.Text = ReadExcelData(tbPath.Text);
                //tbInfo.Text = data.ToString();
                Add2DB(data);

            }
            catch (Exception err)
            {
                tbInfo.Text = $"{err.Message}{ Environment.NewLine} {_i + 4}, {_currentLine}";
                //tbInfo.Text = err.Message;
            }

        }

        private List<string> ReadExcelData(string path, int ignoreLine)
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

                for (int row = ignoreLine; row <= rowCount; row++)//在excel表中第一行是标题，所以数据是从第二行开始的。
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

            //Add2DB(data);
            //return sb.ToString();
            return data;
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
                if (IsEmpty(item, 7))
                    continue;

                CheckAllValue(item, 7);

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
                    var community = context.Communitys.SingleOrDefault(s => s.Name == item[0].Replace("社区", ""));
                    if (community == null)
                    {
                        //CheckValue(item,0);
                        community = new Community { Name = item[0].Replace("社区", "") };
                        community.Street = street;
                        //street.Communities.Add(community);
                        context.Communitys.Add(community);
                    }

                    //网格
                    var netGrid = context.NetGrids.SingleOrDefault(s => s.Community.Id == community.Id && s.Name == item[1].Replace("网格", ""));
                    if (netGrid == null)
                    {
                        // CheckValue(item, 1);
                        netGrid = new NetGrid { Name = item[1].Replace("网格", "") };
                        netGrid.Community = community;
                        context.NetGrids.Add(netGrid);
                    }


                    //小区
                    var subdivision = context.Subdivisions.SingleOrDefault(s => s.Name == item[3].Replace("小区", ""));
                    if (subdivision == null)
                    {
                        //CheckValue(item,3);
                        subdivision = new Subdivision { Name = item[3].Replace("小区", "") };
                        subdivision.Street = street;
                        subdivision.Community = community;
                        context.Subdivisions.Add(subdivision);
                    }

                    //楼栋

                    //var building = netGrid.Buildings.SingleOrDefault(s => s.Name == item[4]);
                    var building = context.Buildings.FirstOrDefault(s => s.NetGrid.Id == netGrid.Id && s.Address == item[2] && s.Name == item[4].Replace("栋", ""));
                    if (building == null)
                    {
                        building = new Building
                        {
                            Name = item[4].Replace("栋", ""),
                            //房屋地址
                            Address = item[2],
                        };
                        building.NetGrid = netGrid;
                        building.Subdivision = subdivision;
                        context.Buildings.Add(building);
                        //subdivision.Buildings.Add(building);
                    }

                    //房屋
                    string roomName = $"{item[5].Replace("单元", "")}-{item[6].Replace("号", "")}";
                    var room = context.Rooms.SingleOrDefault(r => r.Building.Id == building.Id && r.Name == roomName);
                    if (room == null)
                    {
                        room = new Room
                        {
                            Name = roomName,
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
                        companyinfo.Room = room;
                        context.CompanyInfos.Add(companyinfo);
                    }



                    //人
                    var person = context.Persons.SingleOrDefault(p => p.PersonId == item[20]);
                    //检测空名空身份证号
                    if (!CheckItem(item))
                        continue;
                    //检测同身份证号不同名
                    if (!CheckPerson(person, item))
                        continue;

                    if (person == null)
                    {
                        person = new Person
                        {
                            Name = item[18],
                            EthnicGroups = item[19].Replace("族", ""),
                            PersonId = item[20],
                            Phone = item[21],
                            //户籍地址
                            DomicileAddress = item[22],

                            Company = item[27],
                            PoliticalState = item[28],
                            OrganizationalRelation = item[29],
                            IsOverseasChinese = (item[30] == "是"),
                            MerriedStatus = item[31],
                        };
                        // person.CompanyInfo = companyinfo;
                        context.Persons.Add(person);

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
                    //personroom 人房信息  如果同一个身份证同一个房间号视为重复
                    var personHouse = context.PersonRooms.SingleOrDefault(r => r.PersonId == person.PersonId && r.Room.Name == roomName);
                    if(personHouse == null)
                    {
                         personHouse = new PersonRoom
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
                       
                    }
                    context.SaveChanges();
                    _preItem = item;
                }
            }
            tbInfo.Text = "Add personroomdata OK!";
            tbInfo_err.Text += _errorMessage;
        }

        private void CheckAllValue(string[] items, int count)
        {
            //if (items.Length < count)
            // count = items.Length;

            for (int i = 0; i < count; i++)
            {
                string value = items[i].Trim();
                if (value == "")
                {
                    items[i] = _preItem[i];
                }

            }

        }

        private bool IsEmpty(string[] item, int v)
        {
            for (int i = 0; i < v; i++)
            //foreach (var i in item)
            {
                string value = item[i].Trim();
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

                string message = InitDataHelper.AddData(communiteName, gridUser, count);
                tbInfo.Text = message;
            }
            catch (Exception err)
            {
                tbInfo_err.Text += err.Message;
            }
        }
        //添加坐标
        private void bn_AddCoordinates_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> data = ReadExcelData(tbPath.Text, 2);
                Add2Coordinate(data);

            }
            catch (Exception err)
            {
                tbInfo.Text = $"{err.Message}{ Environment.NewLine} {_i + 2}, {_currentLine}";
                //tbInfo.Text = err.Message;
            }
        }

        private void Add2Coordinate(List<string> data)
        {
            for (_i = 0; _i < data.Count; _i++)
            {
                _currentLine = data[_i];
                string[] item = _currentLine.Split(',');
                using (var context = new StreetContext())
                {
                    string roomName = $"{item[7]}-{item[8]}";
                    //                经纬度坐标                                  //社区name——小区alias——楼栋id——房间号
                    var room1 = context.Rooms.FirstOrDefault(r => r.Building.Address== item[4] && r.Building.Name == item[6] && r.Name == roomName);              
                    if (room1 != null)
                    {
                        //经纬度
                        room1.Longitude = Convert.ToDouble(item[9]);
                        room1.Latitude = Convert.ToDouble(item[10]);
                        //楼高
                        double h = (Convert.ToDouble(item[0]) + Convert.ToDouble(item[1]) / 2);
                        room1.Height = Math.Round(h, 2);
                    }
                    context.SaveChanges();
                    tbInfo.Text = "Add Coordinate OK!";
                    //return "Add Coordinate OK!";
                }
            }
        }

        private void bn_AddAlias_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> data = ReadExcelData(tbPath.Text, 1);
                Add2Alias(data);

            }
            catch (Exception err)
            {
                tbInfo.Text = $"{err.Message}{ Environment.NewLine} {_i + 2}, {_currentLine}";
            }
        }

        private void Add2Alias(List<string> data)
        {
            for (_i = 0; _i < data.Count; _i++)
            {
                _currentLine = data[_i];
                string[] item = _currentLine.Split(',');
                using (var context = new StreetContext())
                {
                    var subdivision1 = context.Subdivisions.SingleOrDefault(s => s.Name == item[0]);
                    if (subdivision1 != null)
                    {
                        //CheckValue(item,3);
                        subdivision1.Alias = item[1];
                        //subdivision1.Community.Id = int.Parse(item[3]);

                    }

                    context.SaveChanges();
                    tbInfo.Text = "Add alias OK!";
                    //return "Add Coordinate OK!";
                }
            }
        }

        private void bn_Moni_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new StreetContext())
                {
                    foreach (var item1 in context.Persons)
                    {
                        item1.PersonId = item1.Id.ToString();
                        item1.Phone = item1.Id.ToString();

                    }
                    context.SaveChanges();
                    tbInfo.Text = "OK!";
                }
            }
            catch (Exception err)
            {
                tbInfo.Text = err.Message;
            }
        }
        //导入房间
        private void ImportRooms(List<string> data)
        {
            for (_i = 0; _i < data.Count; _i++)
            {
                _currentLine = data[_i];
                string[] item = _currentLine.Split(',');

                string comName = item[0].Trim().Replace("社区", "");
                string netName = item[1].Trim().Replace("网格", "");
                string adressName = item[2].Trim();
                string buildingName = item[4].Trim().Replace("栋", "");
                string unit = item[5].Trim().Replace("单元", "");
                string roomNO = item[6].Trim();
                string roomN = unit + "-" + roomNO;

                using (var context = new StreetContext())
                {

                    var netGrid = context.NetGrids.FirstOrDefault(n => n.Community.Name == comName && n.Name == netName);

                    var building = context.Buildings.FirstOrDefault(s => s.Address == adressName && s.Name == buildingName);
                    if(building == null)
                    {
                        building = new Building
                        {
                            Name = buildingName,
                            //房屋地址
                            Address = adressName,
                        };
                        building.NetGrid = netGrid;
                        context.Buildings.Add(building);
                    }

                    var importroom = context.Rooms.FirstOrDefault(r => r.Name == roomN && r.Building.Id == building.Id);
                    if (importroom == null)
                    {
                        importroom = new Room
                        {
                            Name = roomN,
                            Category = item[7],
                            Use = item[8],
                            Area = item[10],
                            Other = item[9],

                        };
                        importroom.Building = building;
                        context.Rooms.Add(importroom);
                        context.SaveChanges();
                    }
                }
            }
            tbInfo.Text = "导入room完成!";
            tbInfo_err.Text += _errorMessage;
        }

        private void bn_room_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> data = ReadExcelData(tbPath.Text, 4);
                ImportRooms(data);

            }
            catch (Exception err)
            {
                tbInfo.Text = $"{err.Message}{ Environment.NewLine} {_i + 4}, {_currentLine}";
                //tbInfo.Text = err.Message;
            }
        }
        //数据比对
        private void CheckBuildingsRooms(List<string> data)
        {
            //if (data.Count > 0)
            for (_i = 0; _i < data.Count; _i++)
            {
                _currentLine = data[_i];
                string[] item = _currentLine.Split(',');

                string residence = item[6].Trim();
                string buildingName = item[7].Trim();
                string unit = item[8].Trim();
                string roomNO = item[9].Trim();

                if (residence == "" || buildingName == "" || unit == "" || roomNO == "")
                {
                    _errorMessage += _i + 2 + "小区、楼栋、单元、房间为空" + Environment.NewLine;
                    //tbInfo_err.Text += _i + 2 + "小区、楼栋、单元、房间为空" + Environment.NewLine;
                    continue;
                }

                string roomN = unit + "-" + roomNO;

                using (var context = new StreetContext())
                {
                    //
                    Subdivision sub = context.Subdivisions.FirstOrDefault(s => s.Name == residence || s.Alias.Contains(residence));

                    if (sub == null)
                    {
                        _errorMessage += _i + 2 + "小区未找到" + Environment.NewLine;
                        // tbInfo.Text += "小区没找到， " + _currentLine + Environment.NewLine;
                        continue;
                    }

                    //
                    var building = context.Buildings.FirstOrDefault(s => (s.Subdivision.Id == sub.Id ||s.Subdivision.Alias.Contains(residence)) && (s.Name == buildingName || s.Alias.Contains(buildingName)));
                    if (building == null)
                    {
                        _errorMessage += _i + 2 + "楼栋未找到" + Environment.NewLine;
                        continue;
                    }

                    var room = context.Rooms.SingleOrDefault(r => r.Building.Id == building.Id && r.Name == roomN);
                    if (room == null)
                    {
                        _errorMessage += _i + 2 + "房间未找到" + Environment.NewLine;
                        continue;
                    }
                    //context.SaveChanges();
                }
            }
            tbInfo.Text = "比对完成!";
            tbInfo_err.Text += _errorMessage;
        }
        private void bn_bidui_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> data = ReadExcelData(tbPath.Text, 2);
                CheckBuildingsRooms(data);

            }
            catch (Exception err)
            {
                tbInfo.Text = $"{err.Message}{ Environment.NewLine} {_i + 2}, {_currentLine}";
                //tbInfo.Text = err.Message;
            }
        }

       
    }
}


