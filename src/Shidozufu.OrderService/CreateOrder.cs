using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shidozufu.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shidozufu.OrderService
{
    public class CreateOrder : ICreateOrder
    {
        private readonly ILogger<CreateOrder> _logger;
        private readonly string _connectionString;

        public CreateOrder(
            IOptions<DatabaseOptions> options,
            ILogger<CreateOrder> logger) =>
            (_logger, _connectionString) = (logger, options.Value.ConnectionString);

        public async Task<int> CreateAsync(
            OrderDetail orderDetail, 
            CancellationToken ct = default)
        {
            using var connection = new SqlConnection(_connectionString);
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync(ct);

            using var transaction = await connection.BeginTransactionAsync(ct);
            try
            {
                var id = await connection.QuerySingleAsync<int>("CREATE_ORDER",
                    new
                    {
                        userId = 1,
                        userName = orderDetail.User
                    },
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                await connection.ExecuteAsync("CREATE_ORDER_DETAILS",
                    new
                    {
                        orderId = id,
                        productId = orderDetail.ProductId,
                        quantity = orderDetail.Quantity,
                        productName = orderDetail.Name
                    },
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure);
                await transaction.CommitAsync(ct);
                return id;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                await transaction.RollbackAsync(ct);
                return -1;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
