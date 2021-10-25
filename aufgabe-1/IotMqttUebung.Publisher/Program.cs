using IotMqttUebung.Shared;
using System.Threading.Tasks;

namespace IotMqttUebung.Publisher
{
    public static class Program
    {
        private static async Task Main()
        {
            var testclient = new MqttTestClient();

            await testclient.ConnectAsync();

            await testclient.PublishTestMessageAsync();
        }
    }
}
