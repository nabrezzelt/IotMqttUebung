using System;
using System.Threading.Tasks;

namespace IotMqttUebung2.Subscriber
{
    internal static class Program
    {
        private static MqttSubscribeClient _client;

        private static async Task Main()
        {
            _client = new MqttSubscribeClient();

            await _client.ConnectAsync();
            await _client.SubscribeAsync();

            Console.ReadLine();
        }
    }
}
