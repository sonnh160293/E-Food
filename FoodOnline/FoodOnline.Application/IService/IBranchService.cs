using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Domain.Common;

namespace FoodOnline.Application.IService
{
    public interface IBranchService
    {
        Task<PaginatedList<BranchGetDTO>> GetBranchesAsync(BranchSearchRequest branchSearchRequest);
        Task<int> InsertBranchAsync(BranchPostDTO branchPostDTO);
        Task<IEnumerable<BranchDropDownDTO>> GetBranchForDropDown();
        Task<IEnumerable<BranchDetailGetDTO>> GetAllBranch();
    }
}
