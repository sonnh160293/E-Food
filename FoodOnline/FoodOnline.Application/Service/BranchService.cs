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
    public class BranchService : IBranchService
    {
        private readonly ICommonService _commonService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public BranchService(IUnitOfWork unitOfWork, IMapper mapper, SignInManager<ApplicationUser> signInManager, ICommonService commonService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _signInManager = signInManager;
            _commonService = commonService;
        }




        public async Task<int> InsertBranchAsync(BranchPostDTO branchPostDTO)
        {
            var branch = _mapper.Map<Branch>(branchPostDTO);
            branch.CreatedBy = await _commonService.GetCurrentUser();

            try
            {
                var branchId = await _unitOfWork.BranchRepository.InsertBranchAsync(branch);
                return branchId;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        //get all branch
        public async Task<PaginatedList<BranchGetDTO>> GetBranchesAsync(BranchSearchRequest branchSearchRequest)
        {

            //do filter
            var branches = await _unitOfWork.BranchRepository.GetBranchesAsync(b => string.IsNullOrEmpty(branchSearchRequest.Keyword) ||
                                                                                     b.Detail.Contains(branchSearchRequest.Keyword) ||
                                                                                     b.Street.Contains(branchSearchRequest.Keyword) ||
                                                                                     b.District.Contains(branchSearchRequest.Keyword) ||
                                                                                     b.City.Contains(branchSearchRequest.Keyword) ||
                                                                                     b.Name.Contains(branchSearchRequest.Keyword)
                                                                                     );

            if (branchSearchRequest.Status != null)
            {
                branches = branches.Where(b => b.IsActive == branchSearchRequest.Status);
            }

            var branchesDTO = _mapper.Map<List<BranchGetDTO>>(branches);

            //apply paging
            var paginatedBranches = PaginatedList<BranchGetDTO>.Create(branchesDTO, branchSearchRequest.PageIndex, branchSearchRequest.PageSize);


            return paginatedBranches;
        }


        public async Task<IEnumerable<BranchDropDownDTO>> GetBranchForDropDown()
        {
            var branch = await _unitOfWork.BranchRepository.GetBranchesAsync(b => b.IsActive == true);


            return _mapper.Map<IEnumerable<BranchDropDownDTO>>(branch);
        }

        public async Task<IEnumerable<BranchDetailGetDTO>> GetAllBranch()
        {
            var branches = await _unitOfWork.BranchRepository.GetBranchesAsync(b => b.IsActive != false);
            return _mapper.Map<IEnumerable<BranchDetailGetDTO>>(branches);
        }
    }
}
