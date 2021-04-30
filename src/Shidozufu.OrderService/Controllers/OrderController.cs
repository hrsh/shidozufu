using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Shidozufu.EventBus;
using Shidozufu.Shared;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Shidozufu.OrderService.Controllers
{
    [ApiController, Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderDetailsProvider _orderDetailsProvider;
        private readonly ICreateOrder _createOrder;
        private readonly IDeleteOrder _deleteOrder;
        private readonly IPublisher _publisher;

        private const string ROUTING_KEY = "report.order";

        public OrderController(
            IOrderDetailsProvider orderDetailsProvider,
            ICreateOrder createOrder,
            IDeleteOrder deleteOrder,
            IPublisher publisher)
        {
            _orderDetailsProvider = orderDetailsProvider;
            _createOrder = createOrder;
            _deleteOrder = deleteOrder;
            _publisher = publisher;
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

            _publisher.Publish(
                message: JsonConvert.SerializeObject(
                    orderDetail, 
                    Formatting.None, 
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }),
                routingKey: ROUTING_KEY,
                messageAttributes: null,
                exchangeType: ExchangeType.Topic);

            return Ok(id);
        }

        [HttpDelete("{orderId}")]
        public async Task DeleteOrder(int orderId, CancellationToken ct)
        {
            await _deleteOrder.DeleteAsync(orderId, ct);
        }
    }
}