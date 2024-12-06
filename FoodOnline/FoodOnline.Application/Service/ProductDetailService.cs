using AutoMapper;
using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Application.IService;
using FoodOnline.Domain.Abstract;
using FoodOnline.Domain.Entities;

namespace FoodOnline.Application.Service
{
    public class ProductDetailService : IProductDetailService
    {

        private readonly ICommonService _commonService;
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public ProductDetailService(IBranchService branchService, IMapper mapper, IUnitOfWork unitOfWork, ICommonService commonService)
        {
            _branchService = branchService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _commonService = commonService;
        }

        public Task<ResponseModel> DeleteProductDetailAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDetailGetDTO> GetProductDetailByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDetailGetDTO>> GetProductDetailsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDetailGetDTO>> GetProductDetailsByProductAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDatatable<ProductDetailGetDTO>> GetProductDetailsDatatableAsync(RequestDatatable requestDatatable)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> InsertProductDetailAsync(ProductDetailPostDTO ProductDetailPostDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertProductDetailDefaultAsync(int productId)
        {
            try
            {
                var branches = await _branchService.GetBranchForDropDown();
                var productDetails = new List<ProductDetail>();
                foreach (var branch in branches)
                {
                    var productDetail = new ProductDetail()
                    {
                        BranchId = branch.Id,
                        ProductId = productId,
                        CreatedBy = await _commonService.GetCurrentUser(),
                        UnitInStock = 20,
                        IsActive = true
                    };
                    productDetails.Add(productDetail);
                }
                return await _unitOfWork.ProductDetailRepository.InsertListAsync(productDetails);

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public Task<ResponseModel> UpdateProductDetailAsync(ProductDetailPostDTO ProductDetailPostDTO)
        {
            throw new NotImplementedException();
        }
    }
}
