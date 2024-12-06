using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;

namespace FoodOnline.Application.IService
{
    public interface IUserService
    {
        Task<ResponseDatatable<UserGetDTO>> GetAccountPagination(RequestDatatable requestDatatable);
        Task<ResponseModel> InsertAccount(AccountPostDTO accountPostDTO);
        Task<UserGetDTO> GetUserById(string id);
        Task<ResponseModel> Delete(string id);
        Task<CustomerGetDTO> GetCustomerById(string id);

    }
}
