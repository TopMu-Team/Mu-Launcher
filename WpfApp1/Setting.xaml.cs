using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

public class ComboData
{
    public int Id { get; set; }
    public string Value { get; set; }

}
namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Setting : Window
    {
        public Setting()
        {
            this.WindowStyle = WindowStyle.None;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            List<ComboData> ListResolution = new List<ComboData>();
            ListResolution.Add(new ComboData { Id = 0, Value = "800x600" });
            ListResolution.Add(new ComboData { Id = 1, Value = "1024x768" });
            ListResolution.Add(new ComboData { Id = 2, Value = "1024x768" });
            ListResolution.Add(new ComboData { Id = 3, Value = "1280x720 " });
            ListResolution.Add(new ComboData { Id = 4, Value = "1280x800" });
            ListResolution.Add(new ComboData { Id = 5, Value = "1280x960" });
            ListResolution.Add(new ComboData { Id = 6, Value = "1600x900" });
            ListResolution.Add(new ComboData { Id = 7, Value = "1680x1050" });
            ListResolution.Add(new ComboData { Id = 8, Value = "1920x1080" });
            this.Resolution.ItemsSource = ListResolution;
            this.Resolution.DisplayMemberPath = "Value";
            this.Resolution.SelectedValuePath = "Id";

            List<ComboData> ListWindowMode = new List<ComboData>();
            ListWindowMode.Add(new ComboData { Id = 0, Value = "0ff" });
            ListWindowMode.Add(new ComboData { Id = 1, Value = "On" });
            this.WindowMode.ItemsSource = ListWindowMode;
            this.WindowMode.DisplayMemberPath = "Value";
            this.WindowMode.SelectedValuePath = "Id";

            List<ComboData> ListLanguage = new List<ComboData>();
            ListLanguage.Add(new ComboData { Id = 0, Value = "English" });
            this.Language.ItemsSource = ListLanguage;
            this.Language.DisplayMemberPath = "Value";           
            this.Language.SelectedIndex = 0;


            var lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "/LauncherOption.if");
            var c = Char.Parse(":");
            foreach (var line in lines)
            {
                var key = line.ToString().Trim().Split(c).GetValue(0).ToString();
                if (key == "DevModeIndex")
                {
                    var value = line.ToString().Split(c).GetValue(1).ToString();
                    this.Resolution.SelectedValue = value;
                }
                if (key == "WindowMode")
                {
                    this.WindowMode.SelectedValue = line.ToString().Split(c).GetValue(1);
                }
                if(key == "ID")
                {
                    var value = line.ToString().Split(c).GetValue(1).ToString();
                    this.Account.Text = value;
                }
            }            
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var resolution = (ComboData)this.Resolution.SelectedItem;
            UpdateDevModeIndex(resolution.Id.ToString());

            var account = this.Account.Text;
            UpdateAccount(account);

            var windowMode = (ComboData)this.WindowMode.SelectedItem;
            UpdateWindowMode(windowMode.Id.ToString());

            this.Close();
        }
        private void UpdateDevModeIndex(string value)
        {
            var lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "/LauncherOption.if");
            var c = Char.Parse(":");
            for (var i = 0; i < lines.Length; i++)
            {
                var key = lines.GetValue(i).ToString().Trim().Split(c).GetValue(0).ToString();
                if (key == "DevModeIndex")
                {
                    lines.SetValue("DevModeIndex:" + value, i);
                    break;
                }
            }
            File.WriteAllLines(Directory.GetCurrentDirectory() + "/LauncherOption.if", lines.ToArray());
        }
        private void UpdateAccount(string value)
        {
            var lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "/LauncherOption.if");
            var c = Char.Parse(":");
            for (var i = 0; i < lines.Length; i++)
            {
                var key = lines.GetValue(i).ToString().Trim().Split(c).GetValue(0).ToString();
                if (key == "ID")
                {
                    lines.SetValue("ID:" + value, i);
                    break;
                }
            }
            File.WriteAllLines(Directory.GetCurrentDirectory() + "/LauncherOption.if", lines.ToArray());
        }

        private void UpdateWindowMode(string value)
        {
            var lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "/LauncherOption.if");
            var c = Char.Parse(":");
            for (var i = 0; i < lines.Length; i++)
            {
                var key = lines.GetValue(i).ToString().Trim().Split(c).GetValue(0).ToString();
                if (key == "WindowMode")
                {
                    lines.SetValue("WindowMode:" + value, i);
                    break;
                }
            }
            File.WriteAllLines(Directory.GetCurrentDirectory() + "/LauncherOption.if", lines.ToArray());
        }
    }
}
