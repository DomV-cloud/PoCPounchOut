using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Interfaces.Persistance;
using Template.Application.DatabaseContext;
using Template.Application.Services.Logging;
using Template.Contracts.Order;
using Template.Domain.Entities;

namespace Template.Infrastructure.Persistance
{
    public class OrderRepository : IOrderRepository
    {
        private readonly TemplateContext _context;
        private readonly ILoggingService _loggingService;

        public OrderRepository(
            TemplateContext context, 
            ILoggingService loggingService)
        {
            _context = context;
            _loggingService = loggingService;
        }

        public async Task<Order> CreateOrder(CreateOrderRequest orderDto)
        {
            if (orderDto == null || orderDto.Products.Count == 0)
            {
                await _loggingService.LogWarningAsync("Returning empty order");
                return new();
            }

            var order = new Order
            {
                OrderNumber = Guid.NewGuid().ToString(),
                CustomerId = orderDto.CustomerId,
                OrderDate = DateTime.UtcNow,
                OrderItems = [],
                TotalPrice = orderDto.TotalPrice
            };

            foreach (var productDto in orderDto.Products)
            {
                var item = await _context.Items
                    .FirstOrDefaultAsync(i => i.Id == productDto.Id);

                if (item == null) continue;

                var orderItem = new OrderItem
                {
                    Order = order,
                    Item = item,
                    ItemPrice = item.Price,
                    Quantity = productDto.Quantity
                };

                await _loggingService.LogInformationAsync("Order item to store: {0}", item.ItemName);

                order.OrderItems.Add(orderItem);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }
    }
}
