using JojaMartAPI.DTOs.OrderDtos;
using JojaMartAPI.Services.Interfaces;

namespace JojaMartAPI.Services
{
    public class OrderServices : IOrderService
    {
        private readonly JojaMartDbContext _dbContext;

        public OrderServices(JojaMartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Order CreateNewOrder(NewOrderDTO order)
        {
            var random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var randString = new string(Enumerable.Repeat(chars, 18)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            var newOrder = new Order
            {
                UserId = order.UserId,
                ProductId = order.ProductId,
                Quantity = order.Quantity,
                OrderDate = DateTime.UtcNow,
                Status = "p",
                ShippingAddress = order.ShippingAddress,
                TotalPrice = order.TotalPrice,
                TrackingNumber = randString,
            };

            return newOrder;

        }


    }
}
