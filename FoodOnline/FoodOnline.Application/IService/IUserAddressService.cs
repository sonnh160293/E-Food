using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;

namespace FoodOnline.Application.IService
{
    public interface IUserAddressService
    {
        Task<IEnumerable<UserAddressGetDTO>> GetAddressesOfUserAsync(string userId);
        Task<UserAddressDetailGetDTO> GetDefaultAddressOfUserAsync(string userId);
        Task<ResponseModel> InsertAddressAsync(UserAddressPostDTO userAddressPostDTO);
        Task<ResponseModel> UpdateAddressAsync(UserAddressPostDTO userAddressPostDTO);
        Task<ResponseModel> DeleteAddressAsync(UserAddressPostDTO userAddressPostDTO);
    }
}
