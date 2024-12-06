using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;

namespace FoodOnline.Application.IService
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryGetDTO>> GetCategoriesAsync();
        Task<ResponseDatatable<CategoryGetDTO>> GetCategoriesPaginationAsync(RequestDatatable requestDatatable);
        Task<CategoryGetDTO> GetCategoryByIdAsync(int id);
        Task<ResponseModel> InsertCategoryAsync(CategoryPostDTO categoryPostDTO);
        Task<ResponseModel> UpdateCategoryAsync(CategoryPostDTO categoryPostDTO);
        Task<ResponseModel> DeleteCategoryAsync(int id);
        Task<IEnumerable<CategoryDropDownDTO>> GetCategoryForDropDown();
    }
}
