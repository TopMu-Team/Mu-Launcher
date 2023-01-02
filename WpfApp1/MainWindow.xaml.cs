using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using TopMu;
using System;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Threading;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Reflection;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string totalMax = "100";
        public string totalMin = "0";

        private bool windowMode = false;

  

        string url = "https://www.topmu.org/AutoUpdate.xml";
        public MainWindow()
        {
            this.WindowStyle = WindowStyle.None;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.DataContext = this;
            
            this.init();

            this.getTimeServer();

            this.getStatusServer();
        }

        private async void getTimeServer()
        {

            DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, (object s, EventArgs ev) =>
            {
                this.serverTime.Content = DateTime.UtcNow.AddHours(4).ToString();
            }, this.Dispatcher);
            timer.Start();
        }
        private async void getStatusServer()
        {
            this.status.Content = "Connecting to server";
            
            await Task.Run(() =>
            {
                
                Parallel.For(0, 2, i =>
                {
                   if ( i == 0)
                    {
                        try
                        {
                            using (var client = new TcpClient(Resource1.url, Int32.Parse(Resource1.connectServerPort))) { };
                            Dispatcher.BeginInvoke((() =>
                            {
                                this.connectServer.Content = "ON";
                                this.connectServer.Foreground = new SolidColorBrush(Color.FromRgb(124, 252, 0));
                            }));
                        }
                        catch (SocketException ex)
                        {
                            Dispatcher.BeginInvoke(() =>
                            {
                                this.status.Content = "Connect to server failed";

                                this.connectServer.Content = "OFF";
                                this.connectServer.Foreground = new SolidColorBrush(Color.FromRgb(250, 128, 114));
                            });
                        }
                    }

                   if (i==1)
                    {
                        try
                        {
                            using (var client = new TcpClient(Resource1.url, Int32.Parse(Resource1.gameServerPort))) ;
                            Dispatcher.BeginInvoke(() =>
                            {
                                this.gameServer.Content = "ON";
                                this.gameServer.Foreground = new SolidColorBrush(Color.FromRgb(124, 252, 0));
                            });
                        }
                        catch (SocketException ex)
                        {
                            Dispatcher.BeginInvoke(() =>
                            {
                                this.status.Content = "Connect to server failed";
                                this.gameServer.Content = "OFF";
                                this.gameServer.Foreground = new SolidColorBrush(Color.FromRgb(250, 128, 114));
                            });
                        }
                    }
                });
               
            });
            this.status.Content = "Connect to server successful";
        }

        public bool WindowMode { get => windowMode; set
            {
                windowMode = value;
                this.updateWindowMode(value == true ? "1" : "0");
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
       

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {               
                Process.Start(@"main.exe");
            } catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string url = "https://topmu.org/registration";
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            var saveSettingDialog = new SaveSetting();
            saveSettingDialog.Show();
        }
        private void init()
        {
            var lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "/LauncherOption.if");
            foreach (var line in lines)
            {
                var key = line.ToString().Trim().Split(":").GetValue(0).ToString();
                if(key == "WindowMode")
                {
                    this.windowMode = line.ToString().Split(":").GetValue(1).Equals("1");
                }

            }
        }

        private void updateWindowMode(string value)
        {
            var lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "/LauncherOption.if");
            
            for(var i = 0; i < lines.Length; i++)
            {
                var key = lines.GetValue(i).ToString().Trim().Split(":").GetValue(0).ToString();
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
