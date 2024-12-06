using AutoMapper;
using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Application.IService;
using FoodOnline.Domain.Abstract;
using FoodOnline.Domain.Common;
using FoodOnline.Domain.Entities;
using FoodOnline.Domain.IService;
using Microsoft.AspNetCore.Http;

namespace FoodOnline.Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductDetailService _productDetailService;
        private readonly IImageService _imageService;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IProductDetailService productDetailService, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productDetailService = productDetailService;
            _imageService = imageService;
        }



        public Task<ResponseModel> DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductGetDTO> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductAsync(p => p.Id == id);
            return _mapper.Map<ProductGetDTO>(product);
        }

        public async Task<ProductPostDTO> GetProductForPost(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductAsync(p => p.Id == id);
            return _mapper.Map<ProductPostDTO>(product);
        }

        public async Task<string> GetProductName(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductAsync(p => p.Id == id);
            return product.Name ?? String.Empty;
        }

        public Task<IEnumerable<ProductGetDTO>> GetProductsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDatatable<ProductGetDTO>> GetProductsDatatableAsync(RequestDatatable requestDatatable)
        {
            var products = await _unitOfWork.ProductRepository.GetProductsAsync(p => string.IsNullOrEmpty(requestDatatable.Keyword) ||
                                                                                p.Name.Contains(requestDatatable.Keyword) ||
                                                                                p.Category.Name.Contains(requestDatatable.Keyword) ||
                                                                                p.Description.Contains(requestDatatable.Keyword)
                                                                                , p => p.Category);
            var productsDTO = _mapper.Map<IEnumerable<ProductGetDTO>>(products);
            var result = productsDTO.Skip(requestDatatable.SkipItems).Take(requestDatatable.PageSize).ToList();
            return new ResponseDatatable<ProductGetDTO>()
            {
                Draw = requestDatatable.Draw,
                Data = result,
                RecordsFiltered = products.ToList().Count,
                RecordsTotal = products.ToList().Count
            };
        }

        public async Task<IEnumerable<ProductCartDTO>> GetProductsInCartAsync(int[] productsId)
        {
            var products = await _unitOfWork.ProductRepository.GetProductsAsync(p => productsId.Contains(p.Id), p => p.Category);
            return _mapper.Map<IEnumerable<ProductCartDTO>>(products);
        }


        public async Task<PaginatedList<ProductGetDTO>> GetProductsPagination(CategorySearchRequest categorySearchRequest)
        {

            //do filter
            var products = await _unitOfWork.ProductRepository.GetProductsAsync(p => (!categorySearchRequest.CategoryId.HasValue ||
                                                                                    p.CategoryId == categorySearchRequest.CategoryId) &&
                                                                                     p.IsActice != false, p => p.Category);


            var productsDTO = _mapper.Map<List<ProductGetDTO>>(products);

            //apply paging
            var paginatedProducts = PaginatedList<ProductGetDTO>.Create(productsDTO, categorySearchRequest.PageIndex, categorySearchRequest.PageSize);


            return paginatedProducts;
        }

        public async Task<ResponseModel> InsertProductAsync(ProductPostDTO productPostDTO)
        {
            try
            {
                var product = _mapper.Map<FoodOnline.Domain.Entities.Product>(productPostDTO);
                var imageUrl = Guid.NewGuid().ToString() + ".png";
                product.ImageURL = imageUrl;
                var result = await _unitOfWork.ProductRepository.InsertAsync(product);
                var success = result > 0;

                if (result > 0)
                {
                    _imageService.SaveImagesAsync(new List<IFormFile> { productPostDTO.Image }, "images/product", imageUrl);
                }

                return new ResponseModel
                {
                    Data = result,
                    Action = Domain.Enums.ActionType.Insert,
                    Message = success ? "Product inserted successfully." : "Failed to insert product.",
                    Status = success
                };

            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Insert,
                    Message = ex.Message,
                    Status = false
                };
            }
        }



        public async Task<ResponseModel> UpdateProductAsync(ProductPostDTO productPostDTO)
        {

            var product = await _unitOfWork.ProductRepository.GetProductAsync(p => p.Id == productPostDTO.Id);
            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }

            try
            {
                var productUpdate = _mapper.Map<Product>(productPostDTO);

                if (productPostDTO.Image != null)
                {
                    var imageUrl = Guid.NewGuid().ToString() + ".png";
                    productUpdate.ImageURL = imageUrl;
                    _imageService.SaveImagesAsync(new List<IFormFile> { productPostDTO.Image }, "images/product", imageUrl);
                }
                productUpdate.ImageURL = product.ImageURL;



                var result = await _unitOfWork.ProductRepository.UpdateAsync(productUpdate);
                var success = result > 0;



                return new ResponseModel
                {
                    Data = result,
                    Action = Domain.Enums.ActionType.Update,
                    Message = success ? "Product updated successfully." : "Failed to update product.",
                    Status = success
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Update,
                    Message = ex.Message,
                    Status = false
                };
            }
        }
    }
}
