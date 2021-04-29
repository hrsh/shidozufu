using Shidozufu.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shidozufu.OrderService
{
    public interface IOrderDetailsProvider
    {
        OrderDetail[] Get();

        Task<IEnumerable<OrderDetail>> GetAsync();
    }
}