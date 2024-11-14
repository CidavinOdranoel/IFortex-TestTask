using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{

    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _appDbContext;

        public OrderService(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// Newest order with more than one item
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Order> GetOrder()
        {
            var result =  _appDbContext.Orders
                .Where(x => x.Quantity > 1)
                .OrderByDescending(x => x.CreatedAt)
                .First();
            return result;
        }

        public Task<List<Order>> GetOrders()
        {
            throw new NotImplementedException();
        }
    }
}
