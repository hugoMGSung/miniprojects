using MahApps.Metro.Controls;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SmartHomeMonitoringApp.Logics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
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
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;

namespace SmartHomeMonitoringApp.Views
{
    /// <summary>
    /// DataBaseControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DataBaseControl : UserControl
    {
        public bool IsConnected { get; set; }
        Thread MqttThread { get; set; }  // 없으면 UI컨트롤이 어려워짐

        public DataBaseControl()
        {
            InitializeComponent();
        }


        // 유저컨트롤 로드이벤트 핸들러
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TxbBrokerUrl.Text = Commons.BROKERHOST;
            TxbMqttTopic.Text = Commons.MQTTTOPIC;
            TxtConnString.Text = Commons.MYSQL_CONNSTRING;

            IsConnected = false;     // 아직 접속이 안되었음
            BtnConnDb.IsChecked = false;

            // 실시간 모니터링에서 넘어왔을 때
            if (Commons.MQTT_CLIENT != null && Commons.MQTT_CLIENT.IsConnected)
            {
                IsConnected = true;
                BtnConnDb.Content = "MQTT 연결중";
                BtnConnDb.IsChecked = true;
                Commons.MQTT_CLIENT.MqttMsgPublishReceived += MQTT_CLIENT_MqttMsgPublishReceived;
            }
        }

        // 토글버튼 클릭이벤트 핸들러
        private void BtnConnDb_Click(object sender, RoutedEventArgs e)
        {
            ConnectDB();
        }

        private void ConnectDB()
        {
            if (IsConnected == false)
            {
                // Mqtt 브로커 생성
                Commons.MQTT_CLIENT = new MqttClient(Commons.BROKERHOST);

                try
                {
                    // Mqtt subscribe(구독할) 로직
                    if (Commons.MQTT_CLIENT.IsConnected == false)
                    {
                        // Mqtt 접속
                        Commons.MQTT_CLIENT.MqttMsgPublishReceived += MQTT_CLIENT_MqttMsgPublishReceived;
                        Commons.MQTT_CLIENT.Connect("MONITOR"); // clientId = 모니터
                        Commons.MQTT_CLIENT.Subscribe(new string[] { Commons.MQTTTOPIC },
                                new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE }); // QOS는 네트워크 통신에 옵선
                        UpdateLog(">>> MQTT Broker Connected");

                        BtnConnDb.IsChecked = true;
                        BtnConnDb.Content = "MQTT 연결중";
                        IsConnected = true; // 예외발생하면 true로 변경할 필요 없음
                    }
                }
                catch (Exception ex)
                {
                    UpdateLog($"!!! MQTT Erorr 발생 : {ex.Message}");
                }
            }
            else
            {
                try
                {
                    if (Commons.MQTT_CLIENT.IsConnected)
                    {
                        Commons.MQTT_CLIENT.MqttMsgPublishReceived -= MQTT_CLIENT_MqttMsgPublishReceived;
                        Commons.MQTT_CLIENT.Disconnect();
                        UpdateLog(">>> MQTT Broker Disconnected...");

                        BtnConnDb.IsChecked = false;
                        BtnConnDb.Content = "MQTT 연결종료";
                        IsConnected = false;
                    }
                }
                catch (Exception ex)
                {
                    UpdateLog($"!!! MQTT Erorr 발생 : {ex.Message}");
                }
            }
        }

        private void UpdateLog(string msg)
        {
            // 예외처리 필요!!
            this.Invoke(() => {
                TxtLog.Text += $"{msg}\n";
                TxtLog.ScrollToEnd();
            });
        }

        // Subscribe가 발생할 때 이벤트핸들러
        private void MQTT_CLIENT_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var msg = Encoding.UTF8.GetString(e.Message);
            UpdateLog(msg);
            SetToDataBase(msg, e.Topic); // 실제 DB에 저장처리
        }

        // DB 저장처리 메서드
        private void SetToDataBase(string msg, string topic)
        {
            var currValue = JsonConvert.DeserializeObject<Dictionary<string, string>>(msg);
            if (currValue != null)
            {
                //Debug.WriteLine(currValue["Home_Id"]);
                //Debug.WriteLine(currValue["Room_Name"]);
                //Debug.WriteLine(currValue["Sensing_DateTime"]);
                //Debug.WriteLine(currValue["Temp"]);
                //Debug.WriteLine(currValue["Humid"]);
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(Commons.MYSQL_CONNSTRING))
                    {
                        if (conn.State == System.Data.ConnectionState.Closed) conn.Open();
                        string insQuery = @"INSERT INTO smarthomesensor
                                            (Home_Id,
                                             Room_Name,
                                             Sensing_DateTime,
                                             Temp,
                                             Humid)
                                            VALUES
                                            (@Home_Id,
                                             @Room_Name,
                                             @Sensing_DateTime,
                                             @Temp,
                                             @Humid) ";

                        MySqlCommand cmd = new MySqlCommand(insQuery, conn);
                        cmd.Parameters.AddWithValue("@Home_Id", currValue["Home_Id"]);
                        cmd.Parameters.AddWithValue("@Room_Name", currValue["Room_Name"]);
                        cmd.Parameters.AddWithValue("@Sensing_DateTime", currValue["Sensing_DateTime"]);
                        cmd.Parameters.AddWithValue("@Temp", currValue["Temp"]);
                        cmd.Parameters.AddWithValue("@Humid", currValue["Humid"]);

                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            UpdateLog(">>> DB Insert succeed.");
                        }
                        else
                        {
                            UpdateLog(">>> DB Insert failed."); // 일어날일이 거의 없음
                        }
                    }
                }
                catch (Exception ex)
                {
                    UpdateLog($"!!! DB Erorr 발생 : {ex.Message}");
                }
            }
        }
    }
}
