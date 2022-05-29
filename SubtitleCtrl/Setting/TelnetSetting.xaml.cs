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
using System.Windows.Shapes;

namespace SubtitleCtrl.Setting
{
    /// <summary>
    /// Interaction logic for TelnetSetting.xaml
    /// </summary>
    public partial class TelnetSetting : Window
    {
        TelnetSettingNotify telnetSettingNotify;

        public event DelegateCollection.NoParamDelegate DelSaveConfig;

        public TelnetSetting(TelnetSettingNotify _telnetSettingNotify)
        {
            InitializeComponent();
            this.telnetSettingNotify = _telnetSettingNotify;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this.telnetSettingNotify;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DelSaveConfig();
            MessageBox.Show("Config saved!Restart the app or reconnect please");
        }
    }
}
