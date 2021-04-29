using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;

namespace Shidozufu.EventBus
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly string _connectionString;
        private bool _disposed;
        private readonly IConnection _connection;

        public ConnectionProvider(IOptions<RabbitMqOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
            var _factory = new ConnectionFactory
            {
                Uri = new(_connectionString)
            };
            _connection = _factory.CreateConnection();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _connection?.Close();

            _disposed = true;
        }

        public IConnection GetConnection() => _connection;

        public IConnection Connection => _connection;
    }
}
