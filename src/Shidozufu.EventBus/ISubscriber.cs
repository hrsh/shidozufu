using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shidozufu.EventBus
{
    public interface ISubscriber
    {
        void Subscribe(Func<string, IDictionary<string, object>, bool> callback, string exchange, string queue, string exchangeType, string routingKey, ushort prefetchSize, int timeToLive = 30000);

        void SubscribeAsync(Func<string, IDictionary<string, object>, Task<bool>> callback, string queue, int timeToLive = 30000);
    }
}