using System.Threading;
using System.Threading.Tasks;

namespace Shidozufu.OrderService
{
    public interface IDeleteOrder
    {
        Task DeleteAsync(int orderId, CancellationToken ct = default);
    }
}