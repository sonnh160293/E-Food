using FoodOnline.Application.DTOs;

namespace FoodOnline.Application.IService
{
    public interface IAuthenticateService
    {

        Task<ResponseModel> CheckLogin(string email, string password, bool hasRemember);

    }
}
