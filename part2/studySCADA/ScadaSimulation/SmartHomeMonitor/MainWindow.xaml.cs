using MahApps.Metro.Controls;
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
using System.Diagnostics;
using SmartHomeMonitoringApp.Views;
using MahApps.Metro.Controls.Dialogs;
using SmartHomeMonitoringApp.Logics;
using System.Security.AccessControl;
using System.ComponentModel;
using ControlzEx.Theming;

namespace SmartHomeMonitoringApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        // 끝내기 버튼 클릭이벤트 핸들러
        private void MnuExitProgram_Click(object sender, RoutedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();  // 작업관리자에서 프로세스 종료!
            Environment.Exit(0); // 둘중하나만 쓰면 됨
        }

        // MQTT 시작메뉴 클릭이벤트 핸들러
        private void MnuStartSubscribe_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
        }

        private void BtnExitProgram_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MnuDataBaseMon_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MnuRealTimeMon_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MnuVisualizationMon_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MnuAbout_Click(object sender, RoutedEventArgs e)
        {
        }

    }
}
