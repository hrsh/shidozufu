using System.Collections.Generic;

namespace Shidozufu.EventBus
{
    public interface IPublisher
    {
        void Publish(
            string message,
            string routingKey,
            IDictionary<string, object> messageAttributes,
            string exchangeType,
            string timeToLive = "30000",
            string exchange = null);
    }
}