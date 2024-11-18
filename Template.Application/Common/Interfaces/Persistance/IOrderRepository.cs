using Template.Contracts.Order;
using Template.Domain.Entities;

namespace Template.Application.Common.Interfaces.Persistance
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrder(CreateOrderRequest order);
    }
}
