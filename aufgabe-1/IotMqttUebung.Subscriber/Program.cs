using System;
using System.Threading.Tasks;
using IotMqttUebung.Shared;

namespace IotMqttUebung.Subscriber
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            var testClient = new MqttTestClient();

            await testClient.ConnectAsync();

            await testClient.SubscribeAsync();

            Console.ReadLine();
        }
    }
}
