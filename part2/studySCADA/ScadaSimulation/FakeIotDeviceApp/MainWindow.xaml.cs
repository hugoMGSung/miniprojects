using Bogus;
using FakeIotDeviceApp.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using uPLibrary.Networking.M2Mqtt;
using System.Windows.Documents;
using System;

namespace FakeIotDeviceApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Faker<SensorInfo> FakeHomeSensor { get; set; } = null; // 가짜 스마트홈 센서값 변수
        MqttClient Client { get; set; }
        Thread MqttThread { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            InitFakeData();
        }

        private void InitFakeData()
        {
            var Rooms = new[] { "Bed", "Bath", "Living", "Dining" };

            FakeHomeSensor = new Faker<SensorInfo>()
                .RuleFor(s => s.Home_Id, "D101H703")   // 임의로 픽스된 홈아이디 101동 703호
                .RuleFor(s => s.Room_Name, f => f.PickRandom(Rooms)) // 실행할때마다 방이름이 계속 변경
                .RuleFor(s => s.Sensing_DateTime, f => f.Date.Past(0)) // 현재시각이 생성
                .RuleFor(s => s.Temp, f => f.Random.Float(20.0f, 30.0f)) // 20~30도 사이의 온도값
                .RuleFor(s => s.Humid, f => f.Random.Float(40.0f, 64.0f)); // 40~64% 사이의 습도값
        }

        private async void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtMqttBrokerIp.Text))
            {
                await this.ShowMessageAsync("오류", "브로커아이피를 입력하세요");
                return;
            }

            // 브로커아이피로 접속
            ConnectMqttBroker();
            // 하위의 로직을 무한반복
            StartPublish();            
        }

        // 핵심처리 센싱된 데이터값을 MQTT브로커로 전송
        private void StartPublish()
        {
            MqttThread = new Thread(() =>
            {
                while (true)
                {
                    // 가짜 스마트홈 센서값 생성
                    SensorInfo info = FakeHomeSensor.Generate();
                    // 릴리즈(배포)때는 주석처리/삭제
                    Debug.WriteLine($"{info.Home_Id} / {info.Room_Name} / {info.Sensing_DateTime} / {info.Temp}");                    
                    // 객체 직렬화 (객체데이터를 xml이나 json등의 문자열로 변환)
                    var jsonValue = JsonConvert.SerializeObject(info, Formatting.Indented);
                    // 센서값 MQTT브로커에 전송(Publish)
                    Client.Publish("SmartHome/IoTData/", Encoding.Default.GetBytes(jsonValue));
                    // 스레드와 UI스레드간 충돌이 안나도록 변경
                    this.Invoke(new Action(() => {
                        // RtbLog에 출력                        
                        RtbLog.AppendText($"{jsonValue}\n");
                        RtbLog.ScrollToEnd(); // 스크롤 제일 밑으로 보내기
                    }));
                    
                    // 1초동안 대기
                    Thread.Sleep(1000);
                }
            });
            MqttThread.Start();
        }

        private void ConnectMqttBroker()
        {
            Client = new MqttClient(TxtMqttBrokerIp.Text);
            Client.Connect("SmartHomeDev"); // publish Client ID를 지정
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Client != null && Client.IsConnected == true)
            {
                Client.Disconnect(); // 접속을 끊어주고
            }

            if (MqttThread != null)
            {
                MqttThread.Abort(); // 여기가 없으면프로그램 종료후에도 메모리에 남아있음!!!
            }
        }
    }
}
