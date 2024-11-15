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


            Dictionary<int, int> usersTotalPriceIn2003 = new Dictionary<int, int>();
            foreach (var order in ordersIn2003)
            {
                if (!usersTotalPriceIn2003.ContainsKey(order.UserId))
                {
                    usersTotalPriceIn2003.Add(order.UserId, 0);
                }
                usersTotalPriceIn2003[order.UserId] = order.Price;
                order.User = null;
            }
            

            var userIdWithHighestTotalPrice = usersTotalPriceIn2003
                .OrderByDescending(y => y.Value).First().Key;

            var result = _appDbContext.Users
                .First(x => x.Id == userIdWithHighestTotalPrice);

            result.Orders = null;
            return result;
        }

        public Task<List<User>> GetUsers()
        {
            throw new NotImplementedException();
        }
    }
}
