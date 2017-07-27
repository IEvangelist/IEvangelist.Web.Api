using IEvangelist.Web.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IEvangelist.Web.Api.Services
{
    public interface IOrderService
    {
        /// <summary>Gets all orders</summary>
        Task<IEnumerable<Order>> GetAllOrdersAsync();

        /// <summary>Get order.</summary>
        Task<Order> GetOrderAsync(int id);

        /// <summary>Create order.</summary>
        Task<bool> CreateOrderAsync(Order order);

        /// <summary>Idempotent update.</summary>
        Task<bool> UpdateOrderAsync(int id, Order order);

        /// <summary>Idempotent delete.</summary>
        Task<bool> DeleteOrderAsync(int id);
    }
}