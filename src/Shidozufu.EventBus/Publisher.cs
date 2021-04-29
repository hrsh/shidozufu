using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shidozufu.EventBus
{
    public class Publisher : IPublisher
    {
        private readonly IConnectionProvider _connectionProvider;

        private readonly string _exchange;

        public Publisher(
            IConnectionProvider connectionProvider,
            IOptions<RabbitMqOptions> options)
        {
            _connectionProvider = connectionProvider;
            _exchange = options.Value.PublishOptions.Exchange;
        }

        public void Publish(
            string message,
            string routingKey,
            IDictionary<string, object> messageAttributes,
            string exchangeType,
            string timeToLive = "30000",
            string exchange = null)
        {
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", timeToLive }
            };

            using var model = _connectionProvider.Connection.CreateModel();
            model.ExchangeDeclare(exchange ?? _exchange, exchangeType, arguments: ttl);
            var body = Encoding.UTF8.GetBytes(message);
            var properties = model.CreateBasicProperties();
            properties.Persistent = true;
            properties.Headers = messageAttributes;
            properties.Expiration = timeToLive;

            model.BasicPublish(exchange ?? _exchange, routingKey, properties, body);
        }
    }
}
