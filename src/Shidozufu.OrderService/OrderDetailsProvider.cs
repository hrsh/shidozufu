using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Shidozufu.Shared;

namespace Shidozufu.OrderService
{
    public class OrderDetailsProvider : IOrderDetailsProvider
    {
        private readonly string _connectionString;

        public OrderDetailsProvider(IOptions<DatabaseOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        public OrderDetail[] Get()
        {
            using var connection = new SqlConnection(_connectionString);
            var result = connection.Query<OrderDetail>(@"SELECT 
o.UserName AS [User],
od.ProductName AS [Name],
od.Quantity AS Quantity 
FROM [Order] o 
JOIN [OrderDetail] od ON o.Id = od.OrderID").ToArray();
            return result;
        }

        public async Task<IEnumerable<OrderDetail>> GetAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.QueryAsync<OrderDetail>(@"SELECT 
o.UserName AS [User],
od.ProductName AS [Name],
od.Quantity AS Quantity 
FROM [Order] o 
JOIN [OrderDetail] od ON o.Id = od.OrderID");
            return result;
        }
    }
}