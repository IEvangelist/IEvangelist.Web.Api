using IEvangelist.Web.Api.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IEvangelist.Web.Api.Services
{
    public class OrderService : IOrderService
    {
        private static readonly ConcurrentDictionary<int, Order> _repository =
            new ConcurrentDictionary<int, Order>();

        public Task<bool> CreateOrderAsync(Order order)
            => Task.FromResult(Add(order));

        public Task<bool> DeleteOrderAsync(int id)
            => Task.FromResult(Remove(id));

        public Task<IEnumerable<Order>> GetAllOrdersAsync()
            => Task.FromResult<IEnumerable<Order>>(_repository.Values);

        public Task<Order> GetOrderAsync(int id)
            => Task.FromResult(_repository.ContainsKey(id) ? _repository[id] : null);

        public Task<bool> UpdateOrderAsync(int id, Order order)
            => Task.FromResult(Update(id, order));

        static bool Add(Order order)
        {
            _repository.GetOrAdd(order.Id, order);
            return true;
        }

        static bool Update(int id, Order order)
        {
            if (id != order.Id)
            {
                return false;
            }

            _repository.AddOrUpdate(order.Id, order, (key, existing) => order);
            return true;
        }

        static bool Remove(int id)
            => _repository.ContainsKey(id)
            && _repository.TryRemove(id, out var _);
    }
}