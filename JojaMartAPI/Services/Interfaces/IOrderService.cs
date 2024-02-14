using JojaMartAPI.DTOs.OrderDtos;

namespace JojaMartAPI.Services.Interfaces
{
    public interface IOrderService
    {
        public Order CreateNewOrder(NewOrderDTO newOrder);
    }
}
