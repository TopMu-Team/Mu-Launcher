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
using System.Net.Http;
using System.Security.Policy;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Windows.Media.Effects;

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

public class News
{
    public string Title { get; set; }
    public string Url { get; set; }

}
public class Banner
{
    public string ImgSrc { get; set; }
    public string Url { get; set; }

}
public class Navigation
{
    public string Title { get; set; }
    public string Url { get; set; }
}
public class Translate
{
    public string UpdateSuccess { get; set; }
    public string SettingButton { get; set; }
    public string ServerOffline { get; set; }
    public string ServerOnline { get; set; }
}
public class Data
{
    public List<Banner> Banner { get; set; }
    public List<News> News { get; set; }
    public Translate Translate { get; set; }
    public List<Navigation> Navigation { get; set; }
    public string Logo { get; set; }
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

        private string checkListFileName = "checklist.txt";

        private Translate translate= new Translate();
        private List<Banner> imagePaths = new List<Banner>
        {
           
        };

        public bool WindowMode
        {
            get => windowMode; set
            {
                windowMode = value;
                this.updateWindowMode(value == true ? "1" : "0");
            }
        }


        private int currentIndex = 0;
        private DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            this.WindowStyle = WindowStyle.None;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.DataContext = this;
            this.Init();
            this.StartDownload();
            //this.GetTimeServer();
            this.GetStatusServer();
        }
        private void InitListView(List<News> news)
        {
            

            Dispatcher.BeginInvoke((Action)(() =>
            {
                listView.Items.Clear();
                listView.FontFamily = new FontFamily("Arial");
                listView.FontSize = 14;
                news.ForEach(item =>
                {
                });
                news.ForEach(item =>
                {
                    listView.Items.Add(item);
                });
            }));
          
        }

        private void InitLogoImage(string url)
        {
            if(url != "")
            {
                try
                {
                   
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        BitmapImage bitmapImage = new BitmapImage(new Uri(url));
                        logoImage.Source = bitmapImage;
                    }));
                }
                catch (Exception ex)
                {


                }
            }
           
        }
        private void InitNavigation(List<Navigation> navigations)
        {
           
            Dispatcher.BeginInvoke((Action)(() =>
            {
                navigations.ForEach(navigation => {
                    this.listViewNavigation.Items.Add(navigation);
                });
            }));
        }
        private async void GetTimeServer()
        {

            DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, (object s, EventArgs ev) =>
            {
                this.serverTime.Content = DateTime.UtcNow.AddHours(4).ToString();
            }, this.Dispatcher);
            timer.Start();
        }
   
        private void UpdateTotalProgressBar(long totalSize, long currentSize)
        {
            double percentage = (double)currentSize / (double)totalSize * 100;

            Dispatcher.BeginInvoke((Action)(() =>
            {
                this.TotalProgressText.Text = Math.Round(percentage, 2).ToString() + "%";
                totalProgressBar.Value = int.Parse(Math.Truncate(percentage).ToString());
            }));
        }
        private void GridOfWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            var move = sender as System.Windows.Controls.Grid;
            var win = Window.GetWindow(move);
            win.DragMove();
        }
        private void StartDownload()
        {
            try
            {
                string fileUrl = Resource.downloadUrl + "MD5.txt";
                var tempFolder = Path.Combine(Directory.GetCurrentDirectory(), "Temp");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                this.status.Text = "Updating your client for the latest version";

                long totalSizeNeedDownload = 0;
                long totalSizeDownloaded = 0;
                Thread thread = new Thread(async () => {
                    WebClient getUrl = new WebClient();
                    var text = getUrl.DownloadString(fileUrl).ToString();
                
                    if (File.Exists(checkListFileName))
                    {
                        File.Delete(checkListFileName);
                    }
                    if (Directory.Exists(tempFolder))
                    {
                        Directory.Delete(tempFolder, true);
                    }
                    Directory.CreateDirectory(tempFolder);
                    File.WriteAllText(checkListFileName, text);

                    using (StreamReader reader = new StreamReader(checkListFileName))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null) 
                        {
                            string[] strs = line.Trim().Split(';');
                            var path = strs[0];
                            var currentMd5 = strs[1];
                            var size = Int32.Parse(strs[2]);

                            var newPath = Path.Combine(Directory.GetCurrentDirectory(), path);

                            if (File.Exists(newPath))
                            {
                                var newMd5 = CalculateMD5Checksum(newPath);
                                if (newMd5 != currentMd5) 
                                {
                                    totalSizeNeedDownload += size;
                                }
                            } else
                            {
                                totalSizeNeedDownload += size;
                            }
                        }
                    }
                    using (StreamReader reader = new StreamReader(checkListFileName))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                                string[] strs = line.Trim().Split(';');
                                var path = strs[0];
                                var currentMd5 = strs[1];
                                var size = strs[2];

                                var newPath = Path.Combine(Directory.GetCurrentDirectory(), path);

                                if (File.Exists(newPath))
                                {
                                var newMd5 = CalculateMD5Checksum(newPath);
                                if (newMd5 != currentMd5)
                                {
                                    string downloadUrl = Resource.downloadUrl + "Update/" + path;
                                    string downloadFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Temp", Path.GetFileName(downloadUrl));
                                    string targetFolder = GetContainingFolder(Path.Combine(Directory.GetCurrentDirectory(), path));
                                    using (HttpClient httpClient = new HttpClient())
                                    {
                                        HttpResponseMessage response = await httpClient.GetAsync(downloadUrl);
                                        Dispatcher.BeginInvoke((Action)(() => {
                                            Random random = new Random();
                                            int randomNumber = random.Next(10, 101);
                                            updateProgressBar.Value = randomNumber;
                                            UpdateProgressText.Text = randomNumber.ToString() + "%";
                                        }));
                                        if (response.IsSuccessStatusCode)
                                        {
                                            byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                                            var fileSize = GetFileSize(downloadFilePath);                                            
                                            File.WriteAllBytes(downloadFilePath, fileBytes);                                            
                                            string targetPath = Path.Combine(targetFolder, Path.GetFileName(downloadFilePath));
                                            if (!Directory.Exists(targetFolder))
                                            {
                                                Directory.CreateDirectory(targetFolder);
                                            }
                                            File.Copy(downloadFilePath, targetPath, true);
                                            totalSizeDownloaded += fileSize;
                                            UpdateTotalProgressBar(totalSizeNeedDownload, totalSizeDownloaded);
                                            Dispatcher.BeginInvoke((Action)(() => {                                                
                                                updateProgressBar.Value = 100;
                                                UpdateProgressText.Text = "100" + "%";
                                            }));
                                        }
                                    }

                                }
                                } else
                                {
                                    string downloadUrl = Resource.downloadUrl + "Update/" + path;
                                    string downloadFilePath =  Path.Combine(Directory.GetCurrentDirectory(), "Temp", Path.GetFileName(downloadUrl));
                                    string targetFolder = GetContainingFolder(Path.Combine(Directory.GetCurrentDirectory(), path));
                                    Dispatcher.BeginInvoke((Action)(() => {
                                        this.status.Text = path;
                                    }));

                                using (HttpClient httpClient = new HttpClient())
                                    {
                                        HttpResponseMessage response = await httpClient.GetAsync(downloadUrl);
                                        Dispatcher.BeginInvoke((Action)(() => {
                                            Random random = new Random();
                                            int randomNumber = random.Next(10, 101);
                                            updateProgressBar.Value = randomNumber;
                                            UpdateProgressText.Text = randomNumber.ToString() + "%";
                                        }));
                                    if (response.IsSuccessStatusCode)
                                        {
                                            byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                                            File.WriteAllBytes(downloadFilePath, fileBytes);                                            
                                            string targetPath = Path.Combine(targetFolder, Path.GetFileName(downloadFilePath));
                                            if (!Directory.Exists(targetFolder))
                                            {
                                                Directory.CreateDirectory(targetFolder);
                                            }
                                            File.Copy(downloadFilePath, targetPath, true);
                                            var fileSize = GetFileSize(downloadFilePath);
                                            totalSizeDownloaded += fileSize;
                                            UpdateTotalProgressBar(totalSizeNeedDownload, totalSizeDownloaded);
                                            Dispatcher.BeginInvoke((Action)(() => {
                                                updateProgressBar.Value = 100;
                                                UpdateProgressText.Text = "100" + "%";
                                            }));
                                    }
                                    }
                                }

                            }
                        }
                    if(File.Exists(checkListFileName))
                    {
                        File.Delete(checkListFileName);
                    }
                    if(Directory.Exists(tempFolder))
                    {
                        Directory.Delete(tempFolder, true);
                    }
                    Dispatcher.BeginInvoke((Action)(() => {
                        if(translate.UpdateSuccess != "")
                        {
                            this.status.Text = translate.UpdateSuccess;
                        } else
                        {
                            this.status.Text = "The client has been updated to the latest version successfully";
                        }

                        totalProgressBar.Value = 100;
                        updateProgressBar.Value = 100;
                        UpdateProgressText.Text = "100" + "%";
                        TotalProgressText.Text = "100" + "%";
                    }));
                });
                thread.Start();
            } catch(Exception e)
            {

            }
        }

        static long GetFileSize(string FilePath)
        {
           if (File.Exists(FilePath))
            {
                return new FileInfo(FilePath).Length;
            }
            return 0;
        }

        string GetContainingFolder(string filePath)
        {
            string directoryName = Path.GetDirectoryName(filePath);

            if (directoryName != null)
            {
                return directoryName;
            }
            else
            {
                return "Invalid file path"; // Handle the case where the file path is not valid.
            }
        }

        string CalculateMD5Checksum(string filePath)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = md5.ComputeHash(stream);

                    // Convert the byte array to a hexadecimal string
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
            }
        }


        private async void GetStatusServer()
        {
            await Task.Run(() =>
            {
                try
                {
                    var client = new TcpClient();
                    if (!client.ConnectAsync(Resource.url, Int32.Parse(Resource.connectServerPort)).Wait(3000))
                    {
                        Dispatcher.BeginInvoke((Action)(() =>
                        {
                            if(translate.ServerOffline != "")
                            {
                                this.connectServer.Content = translate.ServerOffline;

                            } else
                            {
                                this.connectServer.Content = "Server is Offline";
                            }
                            this.connectServer.Foreground = new SolidColorBrush(Color.FromRgb(250, 128, 114));

                        }));
                        return;
                    }
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        if (translate.ServerOffline != "")
                        {
                            this.connectServer.Content = translate.ServerOffline;

                        }
                        else
                        {
                            this.connectServer.Content = "Server is Online";
                        }
                        this.connectServer.Content = new SolidColorBrush(Color.FromRgb(124, 252, 0));
                    }));
                }
                catch (SocketException ex)
                {
                    
                }

            });
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

        private void Button_Register(object sender, RoutedEventArgs e)
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

        private void Init()
        {
            try
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
                }

                // Init data from api 
                Thread thread = new Thread(async () =>
                {
                    try
                    {
                        WebClient getUrl = new WebClient();
                        var text = getUrl.DownloadString(Resource.apiUrl).ToString();
                        Data data = JsonConvert.DeserializeObject<Data>(text);

                        foreach (var item in data.Banner)
                        {
                            imagePaths.Add(item);
                        }
                        // Init news
                        this.InitListView(data.News);
                        this.InitNavigation(data.Navigation);
                        this.InitLogoImage(data.Logo);

                        // Init translate
                        this.translate = data.Translate;
                        //Slider show
                        timer.Interval = TimeSpan.FromSeconds(3); // Set the interval for image change
                        timer.Tick += Timer_Tick;
                        Dispatcher.BeginInvoke((Action)(() =>
                        {
                            ShowImage(imagePaths[currentIndex].ImgSrc);
                            // Set setting text
                            if (translate.SettingButton != "")
                            {
                                this.SettingText.Text = translate.SettingButton;
                            }
                        }));
                        timer.Start();
                       
                    } catch (Exception ex) { 
                        MessageBox.Show(ex.Message);
                    }

                });
                thread.Start();
            } catch(Exception ex) { }
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



        //Slider Show
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextImage();
        }

        private void ShowImage(string imagePath)
        {
            imageControl.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
        }

        private void NextImage()
        {
            currentIndex = (currentIndex + 1) % imagePaths.Count;
            ShowImage(imagePaths[currentIndex].ImgSrc);

        }

        private void PreviousImage()
        {
            currentIndex = (currentIndex - 1 + imagePaths.Count) % imagePaths.Count;
            ShowImage(imagePaths[currentIndex].ImgSrc);
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            NextImage();
        }

        private void PreviousButtonClick(object sender, RoutedEventArgs e)
        {
            PreviousImage();
        }

        private void imageControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (currentIndex >= 0 && currentIndex < imagePaths.Count)
                {
                    string url = imagePaths[currentIndex].Url;
                    Process.Start(new ProcessStartInfo(url));
                }
            } catch { }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var baseobj = sender as FrameworkElement;
                var myObject = baseobj.DataContext as News;
                string url = myObject.Url;
                Process.Start(new ProcessStartInfo(url));
            } catch { }
        }

        private void TextBlock_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var baseobj = sender as FrameworkElement;
                var myObject = baseobj.DataContext as Navigation;
                string url = myObject.Url;
                Process.Start(new ProcessStartInfo(url));
            } catch(Exception ex) { }

        }
    }
}
