using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using MQTTnet.Protocol;
using MySqlConnector;

namespace IotMqttUebung2.Subscriber
{
    internal class MqttSubscribeClient
    {
        private MqttFactory _factory;
        private IMqttClient _mqttClient;
        private IMqttClientOptions _options;

        private const string Topic = "iotcourse/T3INF4902";

        private void SetupMqttClient()
        {
            _factory = new MqttFactory();
            _mqttClient = _factory.CreateMqttClient();

            _options = new MqttClientOptionsBuilder()
                       .WithClientId("5F899A6C-A870-40A9-8798-B7647794D3E5")
                       .WithTcpServer("broker.hivemq.com", 1883)
                       .WithCleanSession(false)      //Default
                       .Build();
        }

        public async Task ConnectAsync()
        {
            SetupMqttClient();

            await _mqttClient.ConnectAsync(_options, CancellationToken.None);
        }

        public async Task SubscribeAsync()
        {
            RegisterMessageHandler();

            await _mqttClient.SubscribeAsync(new MqttClientSubscribeOptionsBuilder()
                                             .WithTopicFilter(Topic, MqttQualityOfServiceLevel.AtLeastOnce) //QoS: 1
                                             .Build());
        }

        private void RegisterMessageHandler()
        {
            _mqttClient.UseApplicationMessageReceivedHandler(async e =>
            {
                await InsertPayLoadInDatabaseAsync(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
            });
        }

        private async Task InsertPayLoadInDatabaseAsync(string payloadString)
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            await using var cmd = new MySqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = "INSERT INTO sensor (ts_in, measurement) VALUES (@insert_datetime, @payload_data)";
            cmd.Parameters.AddWithValue("insert_datetime", DateTime.Now);
            cmd.Parameters.AddWithValue("payload_data", payloadString);

            await cmd.ExecuteNonQueryAsync();
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection("Server=localhost;Port=3307;User ID=root;Password=;Database=sensor");
        }
    }
}