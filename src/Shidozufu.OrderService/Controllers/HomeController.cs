using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shidozufu.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace Shidozufu.OrderService.Controllers
{
    [ApiController, Route("api/v1/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IOrderDetailsProvider _orderDetailsProvider;
        private readonly ICreateOrder _createOrder;
        private readonly IDeleteOrder _deleteOrder;

        public HomeController(
            IOrderDetailsProvider orderDetailsProvider,
            ICreateOrder createOrder,
            IDeleteOrder deleteOrder)
        {
            _orderDetailsProvider = orderDetailsProvider;
            _createOrder = createOrder;
            _deleteOrder = deleteOrder;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var t = await _orderDetailsProvider.GetAsync();
            return Ok(t);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(
            [FromBody] OrderDetail orderDetail,
            CancellationToken ct)
        {
            var id = await _createOrder.CreateAsync(orderDetail, ct);
            //publish
            return Ok(id);
        }

        [HttpDelete("{orderId}")]
        public async Task DeleteOrder(int orderId, CancellationToken ct)
        {
            await _deleteOrder.DeleteAsync(orderId, ct);
        }
    }
}