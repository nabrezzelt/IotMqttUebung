using System;
using System.Threading;
using System.Threading.Tasks;

namespace IotMqttUebung2.Publisher
{
    static class Program
    {
        private static MqttPublishClient _client;

        private static async Task Main()
        {
            _client = new MqttPublishClient();

            await _client.ConnectAsync();

            var ct = new CancellationToken();

            Task.Run(async () => await StartPublishingMessage(TimeSpan.FromSeconds(5), ct));

            if (Console.ReadLine() == "exit")
                ct.ThrowIfCancellationRequested();
        }

        private static async Task StartPublishingMessage(TimeSpan interval, CancellationToken cancellationToken)
        {
            while (true)
            {
                await _client.PublishMessageAsync();
                await Task.Delay(interval, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    break;
            }
        }
    }
}
