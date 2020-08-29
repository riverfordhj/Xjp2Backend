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
                tbInfo.Text = err.Message;
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
                            sbRow.Append("空值, ");
                        else
                            sbRow.Append(worksheet.Cells[row, col].Value.ToString() + ", ");
                    }
                    data.Add(sbRow.ToString());
                    sb.Append(sbRow.ToString() + Environment.NewLine);
                }
            }
            Add2DB(data);

            return sb.ToString();
        }

        private void Add2DB(List<string> data)
        {
            if (data.Count > 0)
            {
                string[] item = data[0].Split(',');

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
                    var community = context.Communitys.SingleOrDefault(s => s.Name == item[0]);
                    if (community == null)
                    {
                        community = new Community { Name = item[0] };
                        community.Street = street;
                        //street.Communities.Add(community);
                        context.Communitys.Add(community);
                    }

                    //网格
                    var netGrid = context.NetGrids.SingleOrDefault(s => s.Name == item[1]);
                    if (netGrid == null)
                    {
                        netGrid = new NetGrid { Name = item[1] };
                        netGrid.Community = community;
                        context.NetGrids.Add(netGrid);
                    }


                    //小区
                    var subdivision = context.Subdivisions.SingleOrDefault(s => s.Name == item[3]);
                    if (subdivision == null)
                    {
                        subdivision = new Subdivision { Name = item[3] };
                        subdivision.Street = street;
                        context.Subdivisions.Add(subdivision);
                    }

                    //小区

                    //var building = netGrid.Buildings.SingleOrDefault(s => s.Name == item[4]);
                    var building = context.Buildings.SingleOrDefault(s => s.Community.Id == community.Id && s.Name == item[4]);
                    if (building == null)
                    {
                        building = new Building { Name = item[4] };
                        building.Community = community;
                        building.Subdivision = subdivision;
                        context.Buildings.Add(building);
                        //subdivision.Buildings.Add(building);
                    }

                    //房屋
                    string roomName = $"{item[5]}-{item[6]}";
                    var room = context.Rooms.SingleOrDefault(r => r.Building.Id == building.Id && r.Name == roomName);
                    if (room == null)
                    {
                        room = new Room { Name = roomName };
                        room.Building = building;
                        context.Rooms.Add(room);
                    }


                    //人
                    var person = context.Persons.SingleOrDefault(p => p.PersonId == item[20]);
                    if (person == null)
                    {
                        person = new Person
                        {
                            Name = item[18],
                            EthnicGroups = item[19],
                            PersonId = item[20],
                            Phone = item[21],
                            Address = item[22],

                            Company = item[27],
                            PoliticalState = item[27],
                            OrganizationalRelation = item[28],
                            IsOverseasChinese = (item[29] == "是"),
                            IsMerried = (item[30] == "已婚"),
                        };
                        context.Persons.Add(person);
                    }

                    var personHouse = new PersonRoom
                    {
                        PersonId = item[20],
                        IsHouseholder = (item[23] == "是"),
                        RelationWithHouseholder = item[24],
                        IsOwner = (item[25] == "是"),
                        IsLiveHere = (item[26] == "是"),
                        PopulationCharacter = item[31],
                        LodgingReason = item[32]
                    };

                    personHouse.Person = person;
                    personHouse.Room = room;
                    context.PersonRooms.Add(personHouse);

                    context.SaveChanges();
                }
            }

        }

        private void bnAddInitData_Click(object sender, RoutedEventArgs e)
        {
            string message = InitDataHelper.AddData();
            tbInfo.Text = message;
        }
    }
}
