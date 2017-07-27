using IEvangelist.Web.Api.Models;
using IEvangelist.Web.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace IEvangelist.Web.Api.Controllers
{
    [Route("api/orders")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        //  GET - No defined body semantics.
        //  POST - Body/Form supported.
        //  PUT - Body/Form supported.
        //  DELETE - No defined body semantics.

        //  HEAD - No defined body semantics.
        //  TRACE - Body not supported.
        //  OPTIONS - Body supported but no semantics on usage (maybe in the future).
        //  CONNECT - No defined body semantics

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        #region HTTP GET

        [
            HttpGet("get/{id}")
        ]
        public Task<Order> RouteGet(
            [FromRoute] int id,
            [FromServices] IOrderService orderService)
            // This is the same order service as the one provided in the .ctor
            // I just wanted to demonstrate how you can use the [FromServices] attribute
            => orderService.GetOrderAsync(id);

        [
            HttpGet("queryget")
        ]
        public Task<Order> QueryGet([FromQuery(Name = "identifier")] int id)
            => _orderService.GetOrderAsync(id);

        [
            HttpGet("bodyget")
        ]
        public Task<Order> BodyGet([FromBody] int id)
            => _orderService.GetOrderAsync(id); // No defined body semantics, this will not work!

        [
            HttpGet("formget")
        ]
        public Task<Order> FormGet([FromForm(Name = "identifier")] int id)
            => _orderService.GetOrderAsync(id); // No defined body semantics, this will not work!

        [
            HttpGet("headerget")
        ]
        public Task<Order> HeaderGet([FromHeader(Name = "identifier")] int id)
            // Only valid with string
            // However, the IDE will not tell you that.
            => _orderService.GetOrderAsync(id);

        #endregion // HTTP GET

        #region HTTP POST

        [
            HttpPost("post/{order}")
        ]
        public void RoutePost([FromRoute] Order order)
        {
            // DO NOT DO THIS!
        }

        [
            HttpPost("orders")
        ]
        public async Task<IActionResult> Post([FromBody] Order order)
            => (await _orderService.CreateOrderAsync(order))
                ? (IActionResult)Created($"api/orders/{order.Id}", order)
                : StatusCode(500);

        [
            HttpPost("formpost")
        ]
        public async Task<IActionResult> FormPost([FromForm] Order order)
            => (await _orderService.CreateOrderAsync(order))
                ? Ok()
                : StatusCode(500);

        [
            HttpPost("querypost")
        ]
        public async Task<IActionResult> QueryPost([FromQuery] Order order)
            => (await _orderService.CreateOrderAsync(order))
                ? Ok()
                : StatusCode(500);

        [
            HttpPost("headerpost")
        ]
        public void HeaderPost([FromHeader] string id)
        {
            // DO NOT DO THIS            
        }

        [
            HttpPost("headerarraypost")
        ]
        public void HeaderArrayPost([FromHeader] string[] ids)
        {
            // DO NOT DO THIS
        }

        #endregion // HTTP POST

        #region HTTP PUT

        [
            HttpPut("put/{order}")
        ]
        public void RoutePut([FromRoute] Order order)
        {
            // DO NOT DO THIS!
        }

        [
            HttpPut("api/orders/{id}")
        ]
        public async Task<IActionResult> BodyPut([FromRoute] int id, [FromBody] Order order)
            => (await _orderService.UpdateOrderAsync(id, order))
                ? Ok()
                : StatusCode(500);

        [
            HttpPut("formput")
        ]
        public async Task<IActionResult> FormPut([FromForm] Order order)
            => (await _orderService.UpdateOrderAsync(order.Id, order))
                ? Ok()
                : StatusCode(500);

        [
            HttpPut("queryput")
        ]
        public async Task<IActionResult> QueryPut([FromQuery] Order order)
            => (await _orderService.UpdateOrderAsync(order.Id, order))
                ? Ok()
                : StatusCode(500);

        [
            HttpPut("headerput")
        ]
        public void HeaderPut([FromHeader] string id)
        {
            // DO NOT DO THIS
        }

        [
            HttpPut("headerarrayput")
        ]
        public void HeaderArrayPut([FromHeader] string[] ids)
        {
            // DO NOT DO THIS
        }

        #endregion // HTTP PUT

        #region HTTP DELETE

        [
            HttpDelete("delete/{id}")
        ]
        public async Task<IActionResult> RouteDelete([FromRoute] int id) 
            => (await _orderService.DeleteOrderAsync(id)) 
                ? (IActionResult)Ok() 
                : NoContent();

        [
            HttpDelete("bodydelete")
        ]
        public void BodyDelete([FromBody] Order order)
        {
            // DO NOT DO THIS
        }

        [
            HttpDelete("formdelete")
        ]
        public void FormDelete([FromForm] Order order)
        {
            // DO NOT DO THIS
        }

        [
            HttpDelete("querydelete")
        ]
        public async Task<IActionResult> QueryDelete([FromQuery] int id)
        {
            // Do this only if the value is a non-strong type

            var isDeleted = await _orderService.DeleteOrderAsync(id);
            return isDeleted ? (IActionResult)Ok() : NoContent();
        }

        [
            HttpDelete("headerdelete")
        ]
        public void HeaderDelete([FromHeader] string id)
        {
            // DO NOT DO THIS, no one wants to convert their own ids
        }

        [
            HttpDelete("headerarraydelete")
        ]
        public void HeaderArrayDelete([FromHeader] string[] ids)
        {
            // DO NOT DO THIS
        }

        #endregion // HTTP PUT

        private async Task<IActionResult> ExamineModelStateThenExecuteAsync<T>(
            Func<Order, Task<T>> executeAsync,
            Order order)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();

                if (ModelState.IsValid)
                {
                    // Examine the ModelState when debugging...
                    // Explore its properties, etc.
                }

            }            

            try
            {
                await executeAsync(order);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}