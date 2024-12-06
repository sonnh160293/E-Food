using AutoMapper;
using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Application.IService;
using FoodOnline.Domain.Abstract;
using FoodOnline.Domain.Common;
using FoodOnline.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FoodOnline.Application.Service
{
    public class CategoryService : ICategoryService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICommonService _commonService;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, SignInManager<ApplicationUser> signInManager, ICommonService commonService)
        {
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _commonService = commonService;
        }




        public async Task<ResponseModel> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryAsync(c => c.Id == id);
            if (category == null)
            {
                throw new NotFoundException("Category does not exist.");
            }

            try
            {

                category.LastModifiedBy = await _commonService.GetCurrentUser();


                int result = await _unitOfWork.CategoryRepository.DeleteAsync(category);
                var success = result > 0;

                return new ResponseModel
                {
                    Data = result,
                    Action = Domain.Enums.ActionType.Delete,
                    Message = success ? "Category is deleted successfully." : "Failed to delete category.",
                    Status = success
                };
            }
            catch (Exception ex)
            {
                // Handle all exceptions here
                // Log exception details and return an appropriate response
                return new ResponseModel
                {
                    Action = Domain.Enums.ActionType.Delete,
                    Message = $"An error occurred: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<IEnumerable<CategoryGetDTO>> GetCategoriesAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetCategoriesAsync(c => c.IsActive == true);

            return _mapper.Map<IEnumerable<CategoryGetDTO>>(categories);
        }

        public async Task<ResponseDatatable<CategoryGetDTO>> GetCategoriesPaginationAsync(RequestDatatable requestDatatable)
        {
            // Fetch all categories matching the search keyword
            var categoriesQuery = await _unitOfWork.CategoryRepository.GetCategoriesAsync(c =>
                string.IsNullOrEmpty(requestDatatable.Keyword) || c.Name.Contains(requestDatatable.Keyword));

            // Get the total record count before applying pagination
            var totalRecords = categoriesQuery.ToList().Count;

            // Apply pagination
            var categoriesPaginated = categoriesQuery
                                            .Skip(requestDatatable.SkipItems)
                                            .Take(requestDatatable.PageSize)
                                            .ToList();

            var categoriesDTO = _mapper.Map<IEnumerable<CategoryGetDTO>>(categoriesPaginated);

            // Calculate the filtered record count (after applying filters but before pagination)
            var filteredRecords = categoriesDTO.Count();

            return new ResponseDatatable<CategoryGetDTO>()
            {
                Draw = requestDatatable.Draw,
                RecordsFiltered = totalRecords, // Should be total records if there's no filtering applied
                RecordsTotal = totalRecords, // Total number of records in the database
                Data = categoriesDTO
            };
        }


        public async Task<CategoryGetDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryAsync(c => c.Id == id);
            return _mapper.Map<CategoryGetDTO>(category);
        }

        public async Task<ResponseModel> InsertCategoryAsync(CategoryPostDTO categoryPostDTO)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryPostDTO);
                category.CreatedBy = await _commonService.GetCurrentUser();
                category.CreatedDate = DateTime.Now;

                int result = await _unitOfWork.CategoryRepository.InsertAsync(category);
                var success = result > 0;

                return new ResponseModel
                {
                    Data = result,
                    Action = Domain.Enums.ActionType.Insert,
                    Message = success ? "Category inserted successfully." : "Failed to insert category.",
                    Status = success
                };
            }
            catch (Exception ex)
            {
                // Handle all exceptions here
                // Log exception details and return an appropriate response
                return new ResponseModel
                {
                    Action = Domain.Enums.ActionType.Insert,
                    Message = $"An error occurred: {ex.Message}",
                    Status = false
                };
            }
        }


        public async Task<ResponseModel> UpdateCategoryAsync(CategoryPostDTO categoryPostDTO)
        {

            var cate = await _unitOfWork.CategoryRepository.GetCategoryAsync(c => c.Id == categoryPostDTO.Id);
            if (cate == null)
            {
                throw new NotFoundException("Category does not exist.");
            }

            try
            {
                var category = _mapper.Map<Category>(categoryPostDTO);
                category.LastModifiedBy = await _commonService.GetCurrentUser();


                int result = await _unitOfWork.CategoryRepository.UpdateAsync(category);
                var success = result > 0;

                return new ResponseModel
                {
                    Data = result,
                    Action = Domain.Enums.ActionType.Update,
                    Message = success ? "Category is updated successfully." : "Failed to update category.",
                    Status = success
                };
            }
            catch (Exception ex)
            {
                // Handle all exceptions here
                // Log exception details and return an appropriate response
                return new ResponseModel
                {
                    Action = Domain.Enums.ActionType.Update,
                    Message = $"An error occurred: {ex.Message}",
                    Status = false
                };
            }
        }

        public async Task<IEnumerable<CategoryDropDownDTO>> GetCategoryForDropDown()
        {
            var categories = await _unitOfWork.CategoryRepository.GetCategoriesAsync(c => c.IsActive != false && !c.Name.Equals("Combo"));
            return _mapper.Map<IEnumerable<CategoryDropDownDTO>>(categories);
        }
    }
}
