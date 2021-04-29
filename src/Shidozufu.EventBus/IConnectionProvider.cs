using RabbitMQ.Client;
using System;

namespace Shidozufu.EventBus
{
    public interface IConnectionProvider : IDisposable
    {
        IConnection GetConnection();

        IConnection Connection { get; }
    }
}
