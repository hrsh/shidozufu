using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shidozufu.EventBus
{
    public class Subscriber : ISubscriber
    {
        private readonly IConnectionProvider _connectionProvider;

        private readonly SubscribOptions _subscribOptions;

        public Subscriber(
            IConnectionProvider connectionProvider,
            IOptions<RabbitMqOptions> options)
        {
            _connectionProvider = connectionProvider;
            _subscribOptions = options.Value.SubscribOptions;
        }

        public void Subscribe(
            Func<string, IDictionary<string, object>, bool> callback,
            string exchange,
            string queue,
            string exchangeType,
            string routingKey,
            ushort prefetchSize,
            int timeToLive = 30000)
        {
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", timeToLive }
            };
            using var model = _connectionProvider.Connection.CreateModel();
            model.ExchangeDeclare(exchange, exchangeType, arguments: ttl);
            model.QueueDeclare(queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            model.QueueBind(queue, exchange, routingKey);
            model.BasicQos(0, prefetchSize, false);
            var consumer = new EventingBasicConsumer(model);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                bool success = callback.Invoke(message, e.BasicProperties.Headers);
                if (success)
                {
                    model.BasicAck(e.DeliveryTag, true);
                }
            };

            model.BasicConsume(queue, false, consumer);
        }

        public void SubscribeAsync(
            Func<string, IDictionary<string, object>, Task<bool>> callback,
            string queue,
            int timeToLive = 30000)
        {
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", timeToLive }
            };
            using var model = _connectionProvider.Connection.CreateModel();
            var consumer = new AsyncEventingBasicConsumer(model);
            consumer.Received += async (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                bool success = await callback.Invoke(message, e.BasicProperties.Headers);
                if (success)
                {
                    model.BasicAck(e.DeliveryTag, true);
                }
            };

            model.BasicConsume(queue, false, consumer);
        }
    }
}
