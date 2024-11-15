using System.Linq;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _appDbContext;

        public UserService(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// Return a user with max total price in 2003
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<User> GetUser()
        {
            var ordersIn2003 = _appDbContext.Orders
                .Where(x => x.CreatedAt.Year == 2003).ToList();
            var users = _appDbContext.Users.Where(x => true);


            Dictionary<int, int> usersTotalPriceIn2003 = new Dictionary<int, int>();
            foreach (var order in ordersIn2003)
            {
                if (!usersTotalPriceIn2003.ContainsKey(order.UserId))
                {
                    usersTotalPriceIn2003.Add(order.UserId, 0);
                }
                usersTotalPriceIn2003[order.UserId] = order.Price;
            }
            

            var userIdWithHighestTotalPrice = usersTotalPriceIn2003
                .OrderByDescending(y => y.Value).First().Key;

            var result = _appDbContext.Users
                .First(x => x.Id == userIdWithHighestTotalPrice);

            result.Orders = null;
            return result;
        }

        /// <summary>
        /// Get users with paid orders in 2010
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<User>> GetUsers()
        {
            var paidOrdersIn2010 = _appDbContext.Orders
                .Where(x => x.CreatedAt.Year == 2010)
                .Where(x => x.Status == Enums.OrderStatus.Paid)
                .ToList();
            var users = _appDbContext.Users.ToList();

            HashSet<User> usersWithPaidOrders = new HashSet<User>();

            foreach (var order in paidOrdersIn2010)
            {
                usersWithPaidOrders.Add(users.First(x => x.Id == order.UserId));
            }
            
            var result = usersWithPaidOrders.ToList();

            foreach (var e in result)
            {
                e.Orders = null;
            }

            return result;
        }
    }
}
