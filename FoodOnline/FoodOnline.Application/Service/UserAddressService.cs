using AutoMapper;
using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Application.IService;
using FoodOnline.Domain.Abstract;
using FoodOnline.Domain.Common;
using FoodOnline.Domain.Entities;

namespace FoodOnline.Application.Service
{
    public class UserAddressService : IUserAddressService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICommonService _commonService;

        public UserAddressService(IUnitOfWork unitOfWork, IMapper mapper, ICommonService commonService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _commonService = commonService;
        }

        public async Task<ResponseModel> DeleteAddressAsync(UserAddressPostDTO userAddressPostDTO)
        {
            var userAddress = _unitOfWork.UserAddressRepository.GetAddressAsync(u => u.Id == userAddressPostDTO.Id);
            if (userAddress == null)
            {
                throw new NotFoundException("Address not found");
            }

            try
            {
                var address = _mapper.Map<UserAddress>(userAddressPostDTO);
                address.LastModifiedBy = await _commonService.GetCurrentUser();
                int result = await _unitOfWork.UserAddressRepository.DeleteAddress(address);
                var status = result > 0;
                return new ResponseModel()
                {
                    Status = status,
                    Action = Domain.Enums.ActionType.Delete,
                    Data = result,
                    Message = "Ok"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Action = Domain.Enums.ActionType.Delete,

                    Message = ex.Message
                };
            }
        }

        public async Task<IEnumerable<UserAddressGetDTO>> GetAddressesOfUserAsync(string userId)
        {
            var addresses = await _unitOfWork.UserAddressRepository.GetAddressesAsync(a => a.UserId.Equals(userId));
            return _mapper.Map<IEnumerable<UserAddressGetDTO>>(addresses);
        }

        public async Task<UserAddressDetailGetDTO> GetDefaultAddressOfUserAsync(string userId)
        {
            var address = await _unitOfWork.UserAddressRepository.GetAddressAsync(a => a.UserId.Equals(userId) && a.IsDefault == true);
            return _mapper.Map<UserAddressDetailGetDTO>(address);
        }

        public async Task<ResponseModel> InsertAddressAsync(UserAddressPostDTO userAddressPostDTO)
        {
            try
            {
                var address = _mapper.Map<UserAddress>(userAddressPostDTO);
                address.CreatedBy = await _commonService.GetCurrentUser();
                int result = await _unitOfWork.UserAddressRepository.InsertAddress(address);
                var status = result > 0;
                return new ResponseModel()
                {
                    Status = status,
                    Action = Domain.Enums.ActionType.Insert,
                    Data = result,
                    Message = "Ok"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Action = Domain.Enums.ActionType.Insert,

                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel> UpdateAddressAsync(UserAddressPostDTO userAddressPostDTO)
        {

            var userAddress = _unitOfWork.UserAddressRepository.GetAddressAsync(u => u.Id == userAddressPostDTO.Id);
            if (userAddress == null)
            {
                throw new NotFoundException("Address not found");
            }

            try
            {
                var address = _mapper.Map<UserAddress>(userAddressPostDTO);
                address.LastModifiedBy = await _commonService.GetCurrentUser();
                int result = await _unitOfWork.UserAddressRepository.UpdateAddress(address);
                var status = result > 0;
                return new ResponseModel()
                {
                    Status = status,
                    Action = Domain.Enums.ActionType.Update,
                    Data = result,
                    Message = "Ok"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Action = Domain.Enums.ActionType.Update,

                    Message = ex.Message
                };
            }
        }
    }
}
