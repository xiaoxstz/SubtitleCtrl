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
using Tanzi.File;

namespace SubtitleCtrl.Setting
{
    /// <summary>
    /// Interaction logic for SubTrackSetting.xaml
    /// </summary>
    public partial class SubTrackSetting : Window
    {
        SubTrackSettingNotify subTrackSettingNotify;

        public event DelegateCollection.NoParamDelegate DelSaveConfig;

        public SubTrackSetting(SubTrackSettingNotify _subTrackSettingNotify)
        {
            InitializeComponent();
            this.subTrackSettingNotify = _subTrackSettingNotify;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this.subTrackSettingNotify;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DelSaveConfig();
            MessageBox.Show("Config saved!");
        }
    }
}
