using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;

namespace FoodOnline.Application.IService
{
    public interface IProductDetailService
    {
        Task<IEnumerable<ProductDetailGetDTO>> GetProductDetailsAsync();
        Task<ResponseDatatable<ProductDetailGetDTO>> GetProductDetailsDatatableAsync(RequestDatatable requestDatatable);
        Task<ProductDetailGetDTO> GetProductDetailByIdAsync(int id);
        Task<ResponseModel> InsertProductDetailAsync(ProductDetailPostDTO ProductDetailPostDTO);
        Task<ResponseModel> UpdateProductDetailAsync(ProductDetailPostDTO ProductDetailPostDTO);
        Task<ResponseModel> DeleteProductDetailAsync(int id);
        Task<int> InsertProductDetailDefaultAsync(int productId);
        Task<IEnumerable<ProductDetailGetDTO>> GetProductDetailsByProductAsync(int productId);
    }
}
