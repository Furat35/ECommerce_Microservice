using ECommerce.UI.Helpers;
using ECommerce.UI.Helpers.Filters;
using ECommerce.UI.Models.Dtos.ViewModels.Orders;

namespace ECommerce.UI.Services.Contracts
{
    public interface IOrderService
    {
        Task<(IEnumerable<OrderListModel> products, Metadata pagination)> GetOrders(OrderRequestFilter filters);
        Task<OrderListModel> GetOrderById(string orderId);
    }
}
