using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using IotMqttUebung2.Shared;

namespace IotMqttUebung2.Publisher
{
    class MqttPublishClient
    {
        private MqttFactory _factory;
        private IMqttClient _mqttClient;
        private IMqttClientOptions _options;
        private readonly PerformanceCounter _cpuCounter;

        private const string Topic = "iotcourse/T3INF4902";

        public MqttPublishClient()
        {
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public async Task PublishMessageAsync()
        {
            var message = new MqttApplicationMessageBuilder()
                          .WithTopic(Topic)
                          .WithPayload(GetMessagePayload().AsJsonString())
                          .WithAtLeastOnceQoS() //QoS: 1
                          .WithRetainFlag(false)
                          .Build();

            await _mqttClient.PublishAsync(message, CancellationToken.None);
            Console.WriteLine($"Published: {GetMessagePayload().AsJsonString()}");
        }

        private void SetupMqttClient()
        {
            _factory = new MqttFactory();
            _mqttClient = _factory.CreateMqttClient();

            _options = new MqttClientOptionsBuilder()
                       .WithClientId("6571EA14-808C-4868-8D4D-1F972C4CF336")
                       .WithTcpServer("broker.hivemq.com", 1883)
                       .WithCleanSession(false)      //Default
                       .Build();
        }

        public async Task ConnectAsync()
        {
            SetupMqttClient();

            await _mqttClient.ConnectAsync(_options, CancellationToken.None);
        }

        private MessagePayload GetMessagePayload()
        {
            return new MessagePayload
            {
                CurrentDateTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                Id = "52F9DAC8-0634-4C50-9AE7-17CDBDB5F326",
                Temp = new Random().Next(),
                Battery = 100,
                Load = _cpuCounter.NextValue()
            };
        }
    }
}