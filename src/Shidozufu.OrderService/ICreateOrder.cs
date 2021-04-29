using Shidozufu.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace Shidozufu.OrderService
{
    public interface ICreateOrder
    {
        Task<int> CreateAsync(OrderDetail orderDetail, CancellationToken ct = default);
    }
}
