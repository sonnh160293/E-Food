using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Domain.Common;

namespace FoodOnline.Application.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductGetDTO>> GetProductsAsync();
        Task<ResponseDatatable<ProductGetDTO>> GetProductsDatatableAsync(RequestDatatable requestDatatable);
        Task<ProductGetDTO> GetProductByIdAsync(int id);
        Task<ResponseModel> InsertProductAsync(ProductPostDTO productPostDTO);
        Task<ResponseModel> UpdateProductAsync(ProductPostDTO productPostDTO);
        Task<ResponseModel> DeleteProductAsync(int id);
        Task<ProductPostDTO> GetProductForPost(int id);
        Task<string> GetProductName(int id);

        //customer
        Task<PaginatedList<ProductGetDTO>> GetProductsPagination(CategorySearchRequest categorySearchRequest);
        Task<IEnumerable<ProductCartDTO>> GetProductsInCartAsync(int[] productsId);

    }
}
