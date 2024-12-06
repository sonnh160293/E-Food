using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;

namespace FoodOnline.Application.IService
{
    public interface ICartService
    {
        Task<IEnumerable<CartGetDTO>> GetCartsAsync(string customerId);
        Task<IEnumerable<CartGetDTO>> GetCartItemsByIdsAsync(List<int> checkedItemIds);

        Task<ResponseModel> InsertCartAsync(CartPostDTO cartPostDTO);
        Task<ResponseModel> UpdateCartsAsync(List<CartPostDTO> cartsPostDTO);
        Task<ResponseModel> DeleteCartsAsync(List<int> cartsId);
        Task<ResponseModel> DeleteCartAsync(int cartId);

    }
}
