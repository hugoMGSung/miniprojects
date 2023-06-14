using MahApps.Metro.Controls;
using SmartHomeMonitoringApp.Logics;
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

namespace SmartHomeMonitoringApp.Views
{
    /// <summary>
    /// MqttPopupWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MqttPopupWindow : MetroWindow
    {
        public MqttPopupWindow()
        {
            InitializeComponent();
        }

        // MQTT 접속 버튼클릭이벤트 핸들러
        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            Commons.BROKERHOST = TxtBrokerIp.Text;
            Commons.MQTTTOPIC = TxtTopic.Text;

            this.DialogResult = true; // 
            this.Close();
        }

        // 새 팝업 로드이벤트 핸들러
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TxtBrokerIp.Text = Commons.BROKERHOST;
            TxtTopic.Text = Commons.MQTTTOPIC;
        }
    }
}
