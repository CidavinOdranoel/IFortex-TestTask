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

        /// <summary>
        /// Return orders from active users, sorted by creation date
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Order>> GetOrders()
        {
            var activeUsers = _appDbContext.Users
                .Where(x => x.Status == Enums.UserStatus.Active);

            var result = _appDbContext.Orders
                .Where(x => x.UserId == activeUsers.First(y => y.Id == x.UserId).Id)
                .OrderBy(x => x.CreatedAt)
                .ToList();

            return result;
        }
    }
}
