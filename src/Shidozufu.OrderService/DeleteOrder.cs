using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shidozufu.Shared;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Shidozufu.OrderService
{
    public class DeleteOrder : IDeleteOrder
    {
        private readonly string _connectionString;
        private readonly ILogger<DeleteOrder> _logger;

        public DeleteOrder(
            IOptions<DatabaseOptions> options,
            ILogger<DeleteOrder> logger) =>
            (_logger, _connectionString) = (logger, options.Value.ConnectionString);

        public async Task DeleteAsync(int orderId, CancellationToken ct = default)
        {
            using var connection = new SqlConnection(_connectionString);
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync(ct);

            using var transaction = await connection.BeginTransactionAsync(ct);
            try
            {
                await connection.ExecuteAsync("DELETE FROM OrderDetail WHERE OrderId = @orderId",
                    new { orderId }, transaction: transaction);
                await connection.ExecuteAsync("DELETE FROM [Order] WHERE Id = @orderId",
                    new { orderId }, transaction: transaction);
                transaction.Commit();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                transaction.Rollback();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
