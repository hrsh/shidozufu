using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Shidozufu.EventBus;
using Shidozufu.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shidozufu.ReportService
{
    internal class ReportDataCollector : IHostedService
    {
        private const int DEFAULT_QUANTITY = 100;

        private readonly ISubscriber _subscriber;

        private readonly IMemoryReportStorage _memoryReportStorage;

        public ReportDataCollector(
            ISubscriber subscriber, 
            IMemoryReportStorage memoryReportStorage)
        {
            _subscriber = subscriber;
            _memoryReportStorage = memoryReportStorage;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriber.Subscribe(
                callback: ProcessMessage,
                exchange: "report_exchange",
                queue: "report_queue",
                exchangeType: ExchangeType.Topic,
                routingKey: "report.*",
                prefetchSize: 1,
                timeToLive: 30000);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private bool ProcessMessage(string message, IDictionary<string, object> headers)
        {
            if (message.Contains("Product"))
            {
                var product = JsonConvert.DeserializeObject<Product>(message);
                if (_memoryReportStorage.Get().Any(r => r.ProductName == product.ProductName))
                {
                    return true;
                }
                else
                {
                    _memoryReportStorage.Add(new Report
                    {
                        ProductName = product.ProductName,
                        Count = DEFAULT_QUANTITY
                    });
                }
            }
            else
            {
                var order = JsonConvert.DeserializeObject<Order>(message);
                if (_memoryReportStorage.Get().Any(r => r.ProductName == order.Name))
                {
                    _memoryReportStorage.Get().First(r => r.ProductName == order.Name).Count -= order.Quantity;
                }
                else
                {
                    _memoryReportStorage.Add(new Report
                    {
                        ProductName = order.Name,
                        Count = DEFAULT_QUANTITY - order.Quantity
                    });
                }
            }
            return true;
        }
    }
}