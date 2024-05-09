using ECommerce.UI.Models.Dtos.ViewModels.Discounts;

namespace ECommerce.UI.Services.Contracts
{
    public interface IDiscountService
    {

        Task<DiscountListModel> GetDiscountByProductId(string id);
        Task<string> CreateDiscount(DiscountCreateModel model);
        Task<bool> UpdateDiscount(DiscountUpdateModel model);
        Task<bool> DeleteDiscount(string DiscountId);
    }
}
