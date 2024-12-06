using AutoMapper;
using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Application.IService;
using FoodOnline.Domain.Common;
using FoodOnline.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FoodOnline.Application.Service
{
    public class UserService : IUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICommonService _commonService;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, SignInManager<ApplicationUser> signInManager, ICommonService commonService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _commonService = commonService;
        }



        public async Task<ResponseModel> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                user.LastModifiedBy = await _commonService.GetCurrentUser();
                user.LastModifiedDate = DateTime.Now;
                await _userManager.UpdateAsync(user);
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Delete,

                    Status = true
                };
            }
            return new ResponseModel()
            {
                Action = Domain.Enums.ActionType.Delete,
                Message = "Account not found",
                Status = false
            };
        }

        public async Task<ResponseDatatable<UserGetDTO>> GetAccountPagination(RequestDatatable requestDatatable)
        {
            // Step 1: Apply filtering based on the search keyword and fetch the filtered users
            var filteredUsersQuery = _userManager.Users.Include(u => u.Branch)
                .Where(u =>
                    (string.IsNullOrEmpty(requestDatatable.Keyword) ||
                    u.UserName.Contains(requestDatatable.Keyword) ||
                    u.FullName.Contains(requestDatatable.Keyword) ||
                    u.Email.Contains(requestDatatable.Keyword) ||
                    u.PhoneNumber.Contains(requestDatatable.Keyword) ||
                    u.Branch.Name.Contains(requestDatatable.Keyword))
                    && u.IsDeleted != false);

            // Step 2: Calculate the total number of filtered records
            var recordsFiltered = await filteredUsersQuery.CountAsync();

            // Step 3: Apply pagination
            var pagedUsers = await filteredUsersQuery
                .Skip(requestDatatable.SkipItems)
                .Take(requestDatatable.PageSize)
                .ToListAsync();

            // Step 4: Map to DTO
            var result = _mapper.Map<List<UserGetDTO>>(pagedUsers);

            // Step 5: Retrieve and map roles for each user
            foreach (var userDto in result)
            {
                var user = pagedUsers.FirstOrDefault(u => u.Id == userDto.Id);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    userDto.RoleName = roles.FirstOrDefault() ?? string.Empty;
                }
            }

            // Step 6: Return the response with the correctly calculated counts
            return new ResponseDatatable<UserGetDTO>()
            {
                Draw = requestDatatable.Draw,
                RecordsFiltered = recordsFiltered, // Total records matching the search query
                RecordsTotal = await _userManager.Users.CountAsync(), // Total records without any filtering
                Data = result
            };
        }


        public async Task<ResponseDatatable<UserGetDTO>> GetAccountByBranchPagination(RequestDatatable requestDatatable, int brandId)
        {
            var users = await _userManager.Users.Include(u => u.Branch)
                .Where(u => (string.IsNullOrEmpty(requestDatatable.Keyword) ||
                            u.UserName.Contains(requestDatatable.Keyword) ||
                            u.FullName.Contains(requestDatatable.Keyword) ||
                            u.Email.Contains(requestDatatable.Keyword) ||
                            u.PhoneNumber.Contains(requestDatatable.Keyword)) && u.IsDeleted != false && u.BranchId == brandId)
                .ToListAsync();

            var result = _mapper.Map<List<UserGetDTO>>(users);

            foreach (var userDto in result)
            {
                var user = users.FirstOrDefault(u => u.Id == userDto.Id);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    userDto.RoleName = roles.FirstOrDefault() ?? String.Empty; // Handle if the user has multiple roles or no role
                }
            }



            result = result.Skip(requestDatatable.SkipItems).Take(requestDatatable.PageSize).ToList();

            return new ResponseDatatable<UserGetDTO>()
            {
                Draw = requestDatatable.Draw,
                RecordsFiltered = result.Count,
                RecordsTotal = _userManager.Users.Count(),
                Data = result
            };
        }

        public async Task<ResponseDatatable<UserGetDTO>> GetCustomerByBranchPagination(RequestDatatable requestDatatable)
        {

            var usersInRole = await _userManager.GetUsersInRoleAsync(RoleConstant.CUSTOMER);

            var users = usersInRole.AsQueryable()
                .Include(u => u.Branch)
                .Where(u =>
                    (string.IsNullOrEmpty(requestDatatable.Keyword) ||
                     u.UserName.Contains(requestDatatable.Keyword) ||
                     u.FullName.Contains(requestDatatable.Keyword) ||
                     u.Email.Contains(requestDatatable.Keyword) ||
                     u.PhoneNumber.Contains(requestDatatable.Keyword)) &&
                    u.IsDeleted != false)
                .ToList();


            var result = _mapper.Map<List<UserGetDTO>>(users);

            foreach (var userDto in result)
            {
                var user = users.FirstOrDefault(u => u.Id == userDto.Id);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    userDto.RoleName = roles.FirstOrDefault() ?? String.Empty; // Handle if the user has multiple roles or no role
                }
            }



            result = result.Skip(requestDatatable.SkipItems).Take(requestDatatable.PageSize).ToList();

            return new ResponseDatatable<UserGetDTO>()
            {
                Draw = requestDatatable.Draw,
                RecordsFiltered = result.Count,
                RecordsTotal = _userManager.Users.Count(),
                Data = result
            };
        }

        public async Task<UserGetDTO> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = _mapper.Map<UserGetDTO>(user);
            result.RoleName = (await _userManager.GetRolesAsync(user)).First();
            return result;
        }

        public async Task<ResponseModel> InsertAccount(AccountPostDTO accountPostDTO)
        {
            string errors = "";

            var user = await _userManager.FindByEmailAsync(accountPostDTO.Email);
            if (user != null)
            {
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Insert,
                    Status = false,
                    Message = "User already exist! Try another email account"
                };
            }

            var applicationUser = _mapper.Map<ApplicationUser>(accountPostDTO);
            applicationUser.CreatedBy = await _commonService.GetCurrentUser();
            applicationUser.CreatedDate = DateTime.Now;
            var identityResult = await _userManager.CreateAsync(applicationUser, accountPostDTO.Password);
            if (identityResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(applicationUser, accountPostDTO.RoleName);



                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Insert,
                    Status = true,
                    Message = "Ok",
                    Data = applicationUser.Id
                };

            }
            errors = string.Join("<br/> ", identityResult.Errors.Select(e => e.Description));
            return new ResponseModel()
            {
                Action = Domain.Enums.ActionType.Insert,
                Status = false,
                Message = errors
            };
        }

        public async Task<CustomerGetDTO> GetCustomerById(string id)
        {
            var customer = await _userManager.FindByIdAsync(id);
            return _mapper.Map<CustomerGetDTO>(customer);
        }
    }
}
