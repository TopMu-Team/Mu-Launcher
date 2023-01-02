using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Xml.Linq;

namespace TopMu
{
    /// <summary>
    /// Interaction logic for SaveSetting.xaml
    /// </summary>
    public partial class SaveSetting : Window, INotifyPropertyChanged
    {
        private bool saveBackgroudMusic;
        private bool saveSoundEffect;

        private string screenSolution;
        private string account;

        private bool windowMode = false;


        private bool resolution800x600 = false;
        private bool resolution1024x768 = false;
        private bool resolution1280x720 = false;
        private bool resolution1152x864 = false;
        private bool resolution1280x800 = false;
        private bool resolution1280x960 = false;
        private bool resolution1600x900 = false;
        private bool resolution1680x1050 = false;
        private bool resolution1920x1080 = false;
        Dictionary<string, string> myDictionary = new Dictionary<string, string>(); // make this global

        
        public event PropertyChangedEventHandler? PropertyChanged;

        public Boolean SaveBackgroudMusic { get => saveBackgroudMusic; set => saveBackgroudMusic = value; }
        public bool SaveSoundEffect { get => saveSoundEffect; set => saveSoundEffect = value; }
        public string ScreenSolution { get => screenSolution; set => screenSolution = value; }
        public string Account { get => account; set => account = value; }
        public bool WindowMode { get => windowMode; set => windowMode = value; }
        public bool Resolution800x600 { get => resolution800x600; set {
                resolution800x600 = value;
                OnPropertyChanged(nameof(Resolution800x600));
        }}
        public bool Resolution1024x768 { get => resolution1024x768; set { 
                resolution1024x768 = value;
                OnPropertyChanged(nameof(Resolution1024x768));
         }}
        public bool Resolution1280x720 { get => resolution1280x720; set { 
                resolution1280x720 = value;
                OnPropertyChanged(nameof(Resolution1280x720));
        }}
        public bool Resolution1280x800 { get => resolution1280x800; set { 
                resolution1280x800 = value;
                OnPropertyChanged(nameof(resolution1280x800));
         }}
        public bool Resolution1280x960 { get => resolution1280x960; set {
                resolution1280x960 = value;
                OnPropertyChanged(nameof(resolution1280x960));
         }}
        public bool Resolution1600x900 { get => resolution1600x900; set {
                resolution1600x900 = value;
                OnPropertyChanged(nameof(resolution1600x900));
        }}

        public bool Resolution1152x864 { get => resolution1152x864; set
            {
                resolution1152x864 = value;
                OnPropertyChanged(nameof(Resolution1152x864));
            }
        }
        public bool Resolution1680x1050 { get => resolution1680x1050; set {
                resolution1680x1050 = value;
                OnPropertyChanged(nameof(Resolution1680x1050));
        } }
        public bool Resolution1920x1080 { get => resolution1920x1080; set {
                resolution1920x1080 = value;
                OnPropertyChanged(nameof(Resolution1920x1080));
            }
        }

        public SaveSetting()
        {
            this.WindowStyle = WindowStyle.None;
            this.DataContext = this;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.init();

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void resetAllChkbox()
        {
            this.Resolution800x600=  false;
            this.Resolution1024x768 = false;
            this.Resolution1280x720 = false;
            this.Resolution1280x800 = false;
            this.Resolution1280x960 = false;
            this.Resolution1600x900 = false;
        }
        private void init()
        {
            
            var lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "/LauncherOption.if");
            foreach (var line in lines)
            {
                var key = line.ToString().Trim().Split(":").GetValue(0).ToString();
                if (key == "DevModeIndex")
                {
                    var value = line.ToString().Split(":").GetValue(1).ToString();
                    this.Resolution800x600 = value.Equals("0");
                    this.Resolution1024x768 = value.Equals("1");
                    this.Resolution1152x864 = value.Equals("2");
                    this.Resolution1280x720 = value.Equals("3");
                    this.Resolution1280x800 = value.Equals("4");
                    this.Resolution1280x960 = value.Equals("5");
                    this.Resolution1600x900 = value.Equals("6");
                    this.Resolution1680x1050 = value.Equals("7");
                    this.Resolution1920x1080 = value.Equals("8");
                }

            }

            // Music option ini
            var optionIni = File.ReadAllText(Directory.GetCurrentDirectory() + "/Data/Sound/option.ini");

        }

    
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void resolution800_600ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            this.Resolution1024x768 = false;
            this.Resolution1280x720 = false;
            this.Resolution1280x800 = false;
            this.Resolution1280x960 = false;
            this.Resolution1600x900 = false;
            this.Resolution1152x864 = false;
            this.Resolution1920x1080 = false;
            this.Resolution1680x1050 = false;
        }

        private void resolution1024_768ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            this.Resolution800x600 = false;           
            this.Resolution1280x720 = false;
            this.Resolution1280x800 = false;
            this.Resolution1280x960 = false;
            this.Resolution1600x900 = false;
            this.Resolution1152x864 = false;
            this.Resolution1920x1080 = false;
            this.Resolution1680x1050 = false;
        }

        private void resolution1280_720ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            this.Resolution800x600 = false;
            this.Resolution1024x768 = false;            
            this.Resolution1280x800 = false;
            this.Resolution1280x960 = false;
            this.Resolution1600x900 = false;
            this.Resolution1152x864 = false;
            this.Resolution1920x1080 = false;
            this.Resolution1680x1050 = false;
        }

        private void resolution1280_800ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            this.Resolution800x600 = false;
            this.Resolution1024x768 = false;
            this.Resolution1280x720 = false;            
            this.Resolution1280x960 = false;
            this.Resolution1600x900 = false;
            this.Resolution1152x864 = false;
            this.Resolution1920x1080 = false;
            this.Resolution1680x1050 = false;
        }

        private void resolution1280_960ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            this.Resolution800x600 = false;
            this.Resolution1024x768 = false;
            this.Resolution1280x720 = false;
            this.Resolution1280x800 = false;            
            this.Resolution1600x900 = false;
            this.Resolution1152x864 = false;
            this.Resolution1920x1080 = false;
            this.Resolution1680x1050 = false;
        }

        private void resolution1600_900ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            this.Resolution800x600 = false;
            this.Resolution1024x768 = false;
            this.Resolution1280x720 = false;
            this.Resolution1280x800 = false;
            this.Resolution1280x960 = false;
            this.Resolution1152x864 = false;
            this.Resolution1920x1080 = false;
            this.Resolution1680x1050 = false;
        }

        private void resolution1920_1080ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            this.Resolution800x600 = false;
            this.Resolution1024x768 = false;
            this.Resolution1152x864 = false;
            this.Resolution1280x720 = false;
            this.Resolution1280x800 = false;
            this.Resolution1280x960 = false;
            this.Resolution1600x900 = false;
            this.Resolution1680x1050 = false;

        }

        private void resolution1680_1050ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            this.Resolution800x600 = false;
            this.Resolution1024x768 = false;
            this.Resolution1152x864 = false;
            this.Resolution1280x720 = false;
            this.Resolution1280x800 = false;
            this.Resolution1280x960 = false;
            this.Resolution1600x900 = false;
            this.Resolution1920x1080 = false;
        }

        private void resolution1152_864ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            this.Resolution800x600 = false;
            this.Resolution1024x768 = false;
            this.Resolution1280x720 = false;
            this.Resolution1280x800 = false;
            this.Resolution1280x960 = false;
            this.Resolution1600x900 = false;
            this.Resolution1680x1050 = false;
            this.Resolution1920x1080 = false;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if(this.Resolution800x600 == true)
            {
                this.updateDevModeIndex("0");
            }
            if (this.Resolution1024x768 == true)
            {
                this.updateDevModeIndex("1");
            }
            if (this.Resolution1152x864 == true)
            {
                this.updateDevModeIndex("2");
            }
            if (this.Resolution1280x720 == true)
            {
                this.updateDevModeIndex("3");
            }
            if (this.Resolution1280x800 == true)
            {
                this.updateDevModeIndex("4");
            }
            if (this.Resolution1280x960 == true)
            {
                this.updateDevModeIndex("5");
            }
            if (this.Resolution1600x900 == true)
            {
                this.updateDevModeIndex("6");
            }
            if (this.Resolution1680x1050 == true)
            {
                this.updateDevModeIndex("7");
            }
            if (this.Resolution1920x1080 == true)
            {
                this.updateDevModeIndex("8");
            }
            this.Close();
        }

        private void updateDevModeIndex(string value)
        {
            var lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "/LauncherOption.if");

            for (var i = 0; i < lines.Length; i++)
            {
                var key = lines.GetValue(i).ToString().Trim().Split(":").GetValue(0).ToString();
                if (key == "DevModeIndex")
                {
                    lines.SetValue("DevModeIndex:" + value, i);
                    break;
                }
            }
            File.WriteAllLines(Directory.GetCurrentDirectory() + "/LauncherOption.if", lines.ToArray());
        }

       
    }
}
