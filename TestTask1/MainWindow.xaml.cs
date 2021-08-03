using System;
using System.Collections.Generic;
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
using System.IO;
using Microsoft.Win32;
using Microsoft.EntityFrameworkCore;

namespace TestTask1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OpenFileDialog ofd = new OpenFileDialog();
        string[] s;
        int colvosotr = 0, allhours = 0;
        //Workers worker = new Workers();
        List<Workers> workers = new List<Workers>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tb2.Text = "";
            tb4.Text = "";
            tb6.Text = "";
            tb8.Text = "";
            colvosotr = 0;
            allhours = 0;
            workers.Clear();
            if (ofd.ShowDialog() == true)
            {
                int countstr = File.ReadAllLines(ofd.FileName).Length;
                s = new string[countstr];
                try
                {
                    StreamReader stread = new StreamReader(ofd.FileName);
                    while (!stread.EndOfStream)
                    {
                        for (int i = 0; i < countstr; i++)
                        {
                            s = stread.ReadLine().Split("|", StringSplitOptions.RemoveEmptyEntries);
                            if (s.Length>1 && i > 0)
                            {
                                workers.Add(new Workers() { fio = s[0], age = Convert.ToInt32(s[1]), post = s[2].Trim(), work_hours = (s[3] == "null")? "null" : Convert.ToString(s[3])});
                            }
                        }
                    }

                    foreach (var col in workers)
                    {
                        if ((col.fio.Contains("Иванов") && !col.fio.Contains("Иванова")) == true) colvosotr++;// + " - " + a.Where(x => x == val).Count() + " раз");
                        allhours = (int.TryParse(col.work_hours, out int n) == true) ? allhours += n : allhours += 0;
                    }

                    var minage = workers.OrderBy(x=> x.age).First();

                    tb2.Text = workers.Count().ToString();
                    tb4.Text = colvosotr.ToString();
                    tb6.Text = allhours.ToString();
                    tb8.Text = minage.fio + " " + minage.post;

                    datagrid1.ItemsSource = workers;

                }
                catch (Exception exp) { MessageBox.Show("Что то не так: " + exp.Message, "Ошибка"); };
            }           
        }

        private void b2_Click(object sender, RoutedEventArgs e)
        {
            Info info = new Info();
            tb2.Text = "";
            tb4.Text = "";
            tb6.Text = "";
            tb8.Text = "";
            allhours = 0;
            colvosotr = 0;
            try
            {
                using (var db = new MembersContext())
                {
                    var workrs = db.users.ToList();
                    info.countmembers = workrs.Count();
                    foreach (var col in workrs)
                    {
                        if ((col.fio.Contains("Иванов") && !col.fio.Contains("Иванова")) == true) info.family++;// + " - " + a.Where(x => x == val).Count() + " раз");
                        info.hours = (int.TryParse(col.work_hours, out int n) == true) ? info.hours += n : info.hours += 0;
                    }

                    var minage = workrs.OrderBy(x => x.age).First();

                    tb2.Text = info.countmembers.ToString();
                    tb4.Text = info.family.ToString();
                    tb6.Text = info.hours.ToString();
                    tb8.Text = minage.fio + " " + minage.post;
                    datagrid1.ItemsSource = workrs;
                }
            }
            catch(Exception exp) { MessageBox.Show("Что то не так(2): "+exp.Message,"Ошибка"); }
        }

    }

        public class MembersContext : DbContext
        {
            public DbSet<Workers> users { get; set; }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseMySql("Server=localhost;user=root;password=mysql;Database=members;", new MySqlServerVersion(new Version(8, 0, 26)));
            }
        }

        public class Workers
        {
            //fio | age | post | work_hours
            public int id { get; set; }
            public string fio { get; set; }
            public int age { get; set; }
            public string post { get; set; }
            public string work_hours { get; set; }
        }

    struct Info
    {
        public int countmembers;
        public int family;
        public int hours;
    };
}
/*
create table members(id int primary key, fio varchar(50) not null, age int not null, post varchar(25),work_hours varchar(10));
 
insert into members values(1, 'Иван Иванов Иванович', 43, 'директор', '100');
insert into members values(2, 'Петр Петров', 35, 'менеджер', 'null');
insert into members values(3, 'Сидоров Василий', 29, 'менеджер', '50');
insert into members values(4, 'Иванова Жанна', 25, 'менеджер', '140');
insert into members values(5, 'Артем Иванов', 32, 'программист', '120');
insert into members values(6, 'Иванов Максим', 26, 'дизайнер', '95');





*/