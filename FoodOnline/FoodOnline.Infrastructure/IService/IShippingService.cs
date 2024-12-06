using FoodOnline.Application.DTOs.GetDTO;

namespace FoodOnline.Infrastructure.IService
{
    public interface IShippingService
    {
        Task<FeeDTO> GetShippingFeeAsync(UserAddressDetailGetDTO userAddress);

    }
}
