using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Text;
using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Client.Subscribing;

namespace IotMqttUebung.Shared
{
    public class MqttTestClient
    {
        private MqttFactory _factory;
        private IMqttClient _mqttClient;
        private IMqttClientOptions _options;

        private const string Topic = "iotcourse/internetderdingse";

        public async Task PublishTestMessageAsync()
        {
            var message = new MqttApplicationMessageBuilder()
                          .WithTopic(Topic)
                          .WithPayload("Michael Wendler hier! *wink*")
                          .WithRetainFlag()
                          .Build();

            await _mqttClient.PublishAsync(message, CancellationToken.None);
        }

        private void SetupMqttClient()
        {
            _factory = new MqttFactory();
            _mqttClient = _factory.CreateMqttClient();

            _options = new MqttClientOptionsBuilder()
                       .WithClientId(new Guid().ToString())
                       .WithTcpServer("broker.hivemq.com", 1883)
                       .WithCleanSession()
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

            await _mqttClient.SubscribeAsync(new MqttClientSubscribeOptionsBuilder().WithTopicFilter(Topic).Build());
        }

        private void RegisterMessageHandler()
        {
            _mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();
            });
        }
    }
}
