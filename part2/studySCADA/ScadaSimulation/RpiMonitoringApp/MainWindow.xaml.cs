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
        string DefaultTheme { get; set; } = "Light"; // 기본테마 Light
        string DefaultAccent { get; set; } = "Cobalt"; // 기본액센트 Cobalt

        public MainWindow()
        {
            InitializeComponent();

            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // <Frame> ==> Page.xaml
            // <ContentControl> ==> UserControl.xaml 
            //ActiveItem.Content = new Views.DataBaseControl();
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
            var mqttPopWin = new MqttPopupWindow();
            mqttPopWin.Owner = this;
            mqttPopWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            var result = mqttPopWin.ShowDialog();

            if (result == true)
            {
                var userControl = new Views.DataBaseControl();
                ActiveItem.Content = userControl;
                StsSelScreen.Content = "DataBase Monitoring"; //typeof(Views.DataBaseControl);
            }
        }

        private async void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            // e.Cancel을 true 하고 시작
            e.Cancel = true;

            var mySettings = new MetroDialogSettings
                                {
                                    AffirmativeButtonText = "끝내기",
                                    NegativeButtonText = "취소",
                                    AnimateShow = true,
                                    AnimateHide = true
            };            

            var result = await this.ShowMessageAsync("프로그램 끝내기", "프로그램을 끝내시겠습니까?",
                                                     MessageDialogStyle.AffirmativeAndNegative, mySettings);
            if (result == MessageDialogResult.Negative)
            {
                e.Cancel = true;
            }
            else if (result == MessageDialogResult.Affirmative)
            {
                if (Commons.MQTT_CLIENT != null && Commons.MQTT_CLIENT.IsConnected)
                {
                    Commons.MQTT_CLIENT.Disconnect();                    
                }
                Process.GetCurrentProcess().Kill(); // 가장 확실
            }
        }

        private void BtnExitProgram_Click(object sender, RoutedEventArgs e)
        {
            // 확인메시지 윈도우클로징 이벤트핸들러 호출
            this.MetroWindow_Closing(sender, new CancelEventArgs());
        }

        private void MnuDataBaseMon_Click(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new Views.DataBaseControl();
            StsSelScreen.Content = "DataBase Monitoring"; //typeof(Views.DataBaseControl);
        }

        private void MnuRealTimeMon_Click(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new Views.RealTimeControl();
            StsSelScreen.Content = "RealTime Monitoring";
        }

        private void MnuVisualizationMon_Click(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new Views.VisualizationControl();
            StsSelScreen.Content = "Visualization View";
        }

        private void MnuAbout_Click(object sender, RoutedEventArgs e)
        {
            var about = new About();
            about.Owner = this;
            about.ShowDialog();
        }

        // 모든 테마와 액센트를 전부처리할 체크이벤트핸들러
        private void MnuThemeAccent_Checked(object sender, RoutedEventArgs e)
        {
            // 클릭되는 테마가 라이트인지 다크인지 판단/라이트를 클릭하면 다크는 체크해제, 다크를 클릭하면 라이트를 체크해제
            Debug.WriteLine((sender as MenuItem).Header);
            // 액센트도 체크를하는 값을 나머지 액센트 전부 체크해제

            switch ((sender as MenuItem).Header)
            {
                case "Light":
                    MnuLightTheme.IsChecked = true;
                    MnuDarkTheme.IsChecked = false;
                    DefaultTheme = "Light";
                    break;
                case "Dark":
                    MnuLightTheme.IsChecked = false;
                    MnuDarkTheme.IsChecked = true;
                    DefaultTheme = "Dark";
                    break;
                // 이라내는 액센트
                case "Amber":
                    MnuAccentAmber.IsChecked = true;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;
                    MnuAccentSienna.IsChecked = false;
                    MnuAccentSteel.IsChecked = false;
                    DefaultAccent = "Amber";
                    break;
                case "Blue":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentBlue.IsChecked = true;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;
                    MnuAccentSienna.IsChecked = false;
                    MnuAccentSteel.IsChecked = false;
                    DefaultAccent = "Blue";
                    break;
                case "Brown":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = true;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;
                    MnuAccentSienna.IsChecked = false;
                    MnuAccentSteel.IsChecked = false;
                    DefaultAccent = "Brown";
                    break;
                case "Cobalt":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = true;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;
                    MnuAccentSienna.IsChecked = false;
                    MnuAccentSteel.IsChecked = false;
                    DefaultAccent = "Cobalt";
                    break;
                case "Crimson":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = true;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;
                    MnuAccentSienna.IsChecked = false;
                    MnuAccentSteel.IsChecked = false;
                    DefaultAccent = "Crimson";
                    break;
                case "Cyan":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = true;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;
                    MnuAccentSienna.IsChecked = false;
                    MnuAccentSteel.IsChecked = false;
                    DefaultAccent = "Cyan";
                    break;
                case "Emerald":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentEmerald.IsChecked = true;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;
                    MnuAccentSienna.IsChecked = false;
                    MnuAccentSteel.IsChecked = false;
                    DefaultAccent = "Emerald";
                    break;
                case "Green":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentGreen.IsChecked = true;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;
                    MnuAccentSienna.IsChecked = false;
                    MnuAccentSteel.IsChecked = false;
                    DefaultAccent = "Green";
                    break;
                case "Indigo":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = true;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;
                    MnuAccentSienna.IsChecked = false;
                    MnuAccentSteel.IsChecked = false;
                    DefaultAccent = "Indigo";
                    break;
                case "Lime":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = true;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;
                    MnuAccentSienna.IsChecked = false;
                    MnuAccentSteel.IsChecked = false;
                    DefaultAccent = "Lime";
                    break;
                case "Magenta":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = true;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;
                    MnuAccentSienna.IsChecked = false;
                    MnuAccentSteel.IsChecked = false;
                    DefaultAccent = "Magenta";
                    break;
                case "Mauve":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = true;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;
                    MnuAccentSienna.IsChecked = false;
                    MnuAccentSteel.IsChecked = false;
                    DefaultAccent = "Mauve";
                    break;
                case "Olive":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = true;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;
                    MnuAccentSienna.IsChecked = false;
                    MnuAccentSteel.IsChecked = false;
                    DefaultAccent = "Olive";
                    break;
                case "Orange":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = true;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;
                    MnuAccentSienna.IsChecked = false;
                    MnuAccentSteel.IsChecked = false;
                    DefaultAccent = "Orange";
                    break;
                case "Purple":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = true;
                    MnuAccentRed.IsChecked = false;
                    MnuAccentSienna.IsChecked = false;
                    MnuAccentSteel.IsChecked = false;
                    DefaultAccent = "Purple";
                    break;
                case "Red":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = true;
                    MnuAccentSienna.IsChecked = false;
                    MnuAccentSteel.IsChecked = false;
                    DefaultAccent = "Red";
                    break;
                case "Sienna":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;
                    MnuAccentSienna.IsChecked = true;
                    MnuAccentSteel.IsChecked = false;
                    DefaultAccent = "Sienna";
                    break;
                case "Steel":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;
                    MnuAccentSienna.IsChecked = false;
                    MnuAccentSteel.IsChecked = true;
                    DefaultAccent = "Steel";
                    break;

            }

            ThemeManager.Current.ChangeTheme(this, $"{DefaultTheme}.{DefaultAccent}");
        }
    }
}
