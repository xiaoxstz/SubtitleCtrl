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
using PrimS.Telnet;
using Serilog;
using Tanzi.File;

namespace SubtitleCtrl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Threading.CancellationToken token;

        Client client;

        bool IsConnected = false;

        SubTrackSettingNotify subTrackSettingNotify = new SubTrackSettingNotify();
        TelnetSettingNotify telnetSettingNotify = new TelnetSettingNotify();

        Ini iniFile;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadConfig()
        {
            if (iniFile.ReadValue("Subtrack", "SubEn", out int SubEn, 3))
            {
                subTrackSettingNotify.SubEn = SubEn;
            }
            if (iniFile.ReadValue("Subtrack", "SubZh", out int SubZh, 2))
            {
                subTrackSettingNotify.SubZh = SubZh;
            }
            if (iniFile.ReadValue("Subtrack", "SubNone", out int SubNone, -1))
            {
                subTrackSettingNotify.SubNone = SubNone;
            }

            if (iniFile.ReadValue("Telnet", "Host", out string Host, "localhost"))
            {
                telnetSettingNotify.Host = Host;
            }
            if (iniFile.ReadValue("Telnet", "Port", out int Port, 4212))
            {
                telnetSettingNotify.Port = Port;
            }
            if (iniFile.ReadValue("Telnet", "Password", out string Password, "123"))
            {
                telnetSettingNotify.Password = Password;
            }
        }

        private void SaveConfig()
        {
            bool bSucceed = false;
            bSucceed = iniFile.WriteValue("Subtrack", "SubEn", subTrackSettingNotify.SubEn);
            bSucceed = iniFile.WriteValue("Subtrack", "SubZh", subTrackSettingNotify.SubZh);
            bSucceed = iniFile.WriteValue("Subtrack", "SubNone", subTrackSettingNotify.SubNone);
            bSucceed = iniFile.WriteValue("Telnet", "Host", telnetSettingNotify.Host);
            bSucceed = iniFile.WriteValue("Telnet", "Port", telnetSettingNotify.Port);
            bSucceed = iniFile.WriteValue("Telnet", "Password", telnetSettingNotify.Password);
        }

        private void Connect()
        {
            token = new System.Threading.CancellationToken { };
            client = new Client(telnetSettingNotify.Host, telnetSettingNotify.Port, token);
            IsConnected = client.IsConnected;
            if (IsConnected)
            {
                Task<bool> LoginTask = client.TryLoginAsync("", telnetSettingNotify.Password, 100);
                // TODO:comment out temporarily to avoid being blocked
                //LoginTask.ContinueWith(_ => { Log.Information("completed"); }); 
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            iniFile = new Ini(@".\config.ini");
            this.LoadConfig();
            this.Connect();

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            client?.Dispose();
            this.SaveConfig();
        }

        private void SetSubEn(object sender, RoutedEventArgs e)
        {
            if (IsConnected)
                client.WriteLine($"strack {subTrackSettingNotify.SubEn}");
        }

        private void SetSubZh(object sender, RoutedEventArgs e)
        {
            if (IsConnected)
                client.WriteLine($"strack {subTrackSettingNotify.SubZh}");
        }

        private void SetNoSub(object sender, RoutedEventArgs e)
        {
            if (IsConnected)
                client.WriteLine($"strack {subTrackSettingNotify.SubNone}");
        }

        private void OpenTelnetSetting_Click(object sender, RoutedEventArgs e)
        {
            Setting.TelnetSetting window = new Setting.TelnetSetting(telnetSettingNotify);
            window.DelSaveConfig += this.SaveConfig;
            window.Show();
        }

        private void OpenSubTrackSetting_Click(object sender, RoutedEventArgs e)
        {
            Setting.SubTrackSetting window = new Setting.SubTrackSetting(subTrackSettingNotify);
            window.DelSaveConfig += this.SaveConfig;
            window.Show();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z)
                SetSubEn(null, null);
            else if (e.Key == Key.X)
                SetSubZh(null, null);
            else if (e.Key == Key.C)
                SetNoSub(null, null);
        }

        private void Reconnect_Click(object sender, RoutedEventArgs e)
        {
            client?.Dispose();
            this.Connect();
        }
    }
}
