using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using WpfApp1.Properties;
using System.IO.Compression;
using Path = System.IO.Path;

public class WebClientEx : WebClient
{
    protected override WebRequest GetWebRequest(Uri address)
    {
        var webRequest = (HttpWebRequest)base.GetWebRequest(address);
        if (webRequest != null)
        {
            webRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        }
        return webRequest;
    }
}

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

        private string version = "";
        
        public MainWindow()
        {
            this.WindowStyle = WindowStyle.None;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.StartDownload();
            this.DataContext = this;

            this.init();

            this.GetTimeServer();

            // this.GetStatusServer();
        }

        private async void GetTimeServer()
        {

            DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, (object s, EventArgs ev) =>
            {
                this.serverTime.Content = DateTime.UtcNow.AddHours(4).ToString();
            }, this.Dispatcher);
            timer.Start();
        }
        private void StartDownload()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                this.status.Content = "Updating your game to the latest version";
                Thread thread = new Thread(() => {
                    WebClient getUrl = new WebClient();
                    var text = getUrl.DownloadString(Resource.downloadUrl).ToString();
                    var c = Char.Parse("@");
                    var url = text.Split(c).GetValue(0).ToString();
                    var version = text.Split(c).GetValue(1).ToString();
                    if(this.version.ToString() == version.ToString())
                    {
                        Dispatcher.BeginInvoke((Action)(() =>
                        {
                            updateProgressBar.Value = 100;
                            totalProgressBar.Value = 100;
                            this.status.Content = "Update successful have fun to play";
                        }));
                        return;
                    }
                    this.version = version;
                    WebClient client = new WebClientEx();
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Client_DownloadProgressChanged);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(Client_DownloadFileCompleted);
                    client.DownloadFileAsync(new Uri(url), "ClientPatch.zip");
                });
                thread.Start();
            } catch(Exception e)
            {

            }
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.UpdateVersion(this.version);
            Dispatcher.BeginInvoke((Action)(() => {                
                double percentage = 100;
                this.TotalProgressText.Content = Math.Round(percentage, 2).ToString() + "%";
                totalProgressBar.Value = int.Parse(Math.Truncate(percentage).ToString());
            }));
            var maxProgressPercent = 100;
            var currentFile = 0;
            using (var strm = File.OpenRead("ClientPatch.zip"))
            using (ZipArchive a = new ZipArchive(strm))
            {
                var totalFiles = a.Entries.Where(x => x.Length > 0).Count();

                a.Entries.Where(o => o.Name == string.Empty && !Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), o.FullName)))
                    .ToList()
                    .ForEach(o => {
                        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), o.FullName));
                        currentFile += 1;
                        Dispatcher.BeginInvoke((Action)(() =>
                        {
                            updateProgressBar.Value = (currentFile / totalFiles) * maxProgressPercent;
                            UpdateProgressText.Content = ((currentFile / totalFiles) * maxProgressPercent).ToString() + "%";
                        }));
                    });
                a.Entries.Where(o => o.Name != string.Empty).ToList().ForEach(ac => {
                    ac.ExtractToFile(Path.Combine(Directory.GetCurrentDirectory(), ac.FullName), true);
                    currentFile += 1;
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        updateProgressBar.Value = (currentFile / totalFiles) * maxProgressPercent;
                        UpdateProgressText.Content = ((currentFile / totalFiles) * maxProgressPercent).ToString() + "%";
                    }));

                });
            }
            File.Delete("ClientPatch.zip");
            Dispatcher.BeginInvoke((Action)(() =>
            {
                this.status.Content = "Update successful have fun to play";
            }));
            return;
        }

        void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(()=> {

                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                this.TotalProgressText.Content = Math.Round(percentage, 2).ToString() + "%";
                totalProgressBar.Value = int.Parse(Math.Truncate(percentage).ToString()); 
            }));
        }
        private async void GetStatusServer()
        {
            this.status.Content = "Connecting to server";

            await Task.Run(() =>
            {

                Parallel.For(0, 2, i =>
                {
                    if (i == 0)
                    {
                        try
                        {
                            using (var client = new TcpClient(Resource.url, Int32.Parse(Resource.connectServerPort))) { };
                            Dispatcher.BeginInvoke((Action)(() =>
                            {
                                // this.connectServer.Content = "ON";
                                // this.connectServer.Foreground = new SolidColorBrush(Color.FromRgb(124, 252, 0));
                            }));
                           
                        }
                        catch (SocketException ex)
                        {
                            Dispatcher.BeginInvoke((Action)(() =>
                            {
                                this.status.Content = "Connect to server failed";

                                // this.connectServer.Content = "OFF";
                                // this.connectServer.Foreground = new SolidColorBrush(Color.FromRgb(250, 128, 114));
                            }));
                        }
                    }

                    if (i == 1)
                    {
                        try
                        {
                            using (var client = new TcpClient(Resource.url, Int32.Parse(Resource.gameServerPort))) ;
                            Dispatcher.BeginInvoke((Action)(() =>
                            {
                                this.gameServer.Content = "ON";
                                this.gameServer.Foreground = new SolidColorBrush(Color.FromRgb(124, 252, 0));
                            }));
                        }
                        catch (SocketException ex)
                        {
                            Dispatcher.BeginInvoke((Action)(() =>
                            {
                                this.status.Content = "Connect to server failed";
                                this.gameServer.Content = "OFF";
                                this.gameServer.Foreground = new SolidColorBrush(Color.FromRgb(250, 128, 114));
                            }));
                        }
                    }
                });

            });
            this.status.Content = "Connect to server successful";
        }

        public bool WindowMode
        {
            get => windowMode; set
            {
                windowMode = value;
                this.updateWindowMode(value == true ? "1" : "0");
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Button_Start(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(@"main.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string url = "https://topmu.org/registration";
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }

        private void Button_Minimized(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Close(object sender, RoutedEventArgs e)
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
            var saveSettingDialog = new Setting();
            saveSettingDialog.Show();
        }
        private void init()
        {
            var lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "/LauncherOption.if");
            char c = Char.Parse(":");
            foreach (var line in lines)
            {
                var key = line.ToString().Trim().Split(c).GetValue(0).ToString();
                if (key == "WindowMode")
                {
                    this.windowMode = line.ToString().Split(c).GetValue(1).Equals("1");
                }
                if(key == "Version")
                {
                    this.version = line.ToString().Trim().Split(c).GetValue(1).ToString();
                }
            }
        }

        private void updateWindowMode(string value)
        {
            var lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "/LauncherOption.if");
            char c = Char.Parse(":");
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
        private void UpdateVersion(string value)
        {
            var lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "/LauncherOption.if");
            char c = Char.Parse(":");
            for (var i = 0; i < lines.Length; i++)
            {
                var key = lines.GetValue(i).ToString().Trim().Split(c).GetValue(0).ToString();
                if (key == "Version")
                {
                    lines.SetValue("Version:" + value, i);
                    break;
                }
            }
            File.WriteAllLines(Directory.GetCurrentDirectory() + "/LauncherOption.if", lines.ToArray());
        }
    }
}
