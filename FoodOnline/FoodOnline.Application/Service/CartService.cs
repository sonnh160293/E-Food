using AutoMapper;
using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Application.IService;
using FoodOnline.Domain.Abstract;
using FoodOnline.Domain.Common;
using FoodOnline.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace FoodOnline.Application.Service
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ICommonService _commonService;

        public CartService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            ICommonService commonService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _commonService = commonService;
        }

        public async Task<IEnumerable<CartGetDTO>> GetCartsAsync(string customerId)
        {

            List<CartGetDTO> carts;

            if (!string.IsNullOrEmpty(customerId))
            {
                carts = await GetCartsForLoggedInUserAsync(customerId);
            }
            else
            {
                carts = GetCartsFromSession();
            }

            return carts;
        }

        private async Task<List<CartGetDTO>> GetCartsForLoggedInUserAsync(string customerId)
        {
            var dbCarts = await _unitOfWork.CartRepository.GetCartsAsync(c => c.CustomerId == customerId, c => c.Product);
            var carts = _mapper.Map<List<CartGetDTO>>(dbCarts);

            var sessionCarts = GetCartsFromSession();

            if (sessionCarts is not null && sessionCarts.Count() > 0)
            {
                await MergeSessionCartsWithDbCarts(carts, sessionCarts);



                ClearSessionCarts();
            }

            return carts;
        }

        private List<CartGetDTO> GetCartsFromSession()
        {
            return _commonService.Get<List<CartGetDTO>>(GetCartName() ?? String.Empty) ?? new List<CartGetDTO>();
        }

        private async Task MergeSessionCartsWithDbCarts(List<CartGetDTO> dbCarts, List<CartGetDTO> sessionCarts)
        {
            foreach (var sessionCart in sessionCarts)
            {
                var dbCart = dbCarts.FirstOrDefault(c => c.ProductId == sessionCart.ProductId);
                if (dbCart != null)
                {
                    dbCart.Quantity += sessionCart.Quantity;
                }
                else
                {
                    var cart = _mapper.Map<Cart>(sessionCart);
                    cart.CustomerId = await _commonService.GetCurrentUserId();
                    await _unitOfWork.CartRepository.InsertCartAsync(cart);
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task SaveCartsToDatabaseAsync(List<CartGetDTO> carts)
        {
            var dbCartEntities = _mapper.Map<List<Cart>>(carts);
            await _unitOfWork.CartRepository.UpdateCartsAsync(dbCartEntities);

        }

        private void ClearSessionCarts()
        {
            _httpContextAccessor.HttpContext.Session.Remove(GetCartName());
        }

        public async Task<ResponseModel> InsertCartAsync(CartPostDTO cartPostDTO)
        {
            try
            {
                // Check if the user is logged in

                if (string.IsNullOrEmpty(cartPostDTO.CustomerId))
                {
                    // User is not logged in, manage the cart in the session
                    var sessionCarts = _commonService.Get<List<CartPostDTO>>(GetCartName()) ?? new List<CartPostDTO>();
                    var existingCart = sessionCarts.FirstOrDefault(c => c.ProductId == cartPostDTO.ProductId);

                    if (existingCart != null)
                    {
                        existingCart.Quantity += cartPostDTO.Quantity;
                    }
                    else
                    {
                        sessionCarts.Add(cartPostDTO);
                    }

                    _commonService.Set(GetCartName(), sessionCarts);

                    return new ResponseModel()
                    {
                        Action = Domain.Enums.ActionType.Insert,
                        Message = "Cart updated in session",
                        Status = true
                    };
                }
                else
                {
                    // User is logged in, handle cart in the database
                    var cartExist = await _unitOfWork.CartRepository.GetCartAsync(c => c.ProductId == cartPostDTO.ProductId && c.CustomerId.Equals(cartPostDTO.CustomerId));
                    if (cartExist != null)
                    {
                        cartExist.Quantity += cartPostDTO.Quantity;
                        var resultUpdate = await _unitOfWork.CartRepository.UpdateCartAsync(cartExist);
                        var statusUpdate = resultUpdate > 0;

                        return new ResponseModel()
                        {
                            Action = Domain.Enums.ActionType.Update,
                            Message = statusUpdate ? "Ok" : "Fail",
                            Status = statusUpdate
                        };
                    }
                    var cart = _mapper.Map<Cart>(cartPostDTO);


                    var result = await _unitOfWork.CartRepository.InsertCartAsync(cart);
                    var status = result > 0;

                    return new ResponseModel()
                    {
                        Action = Domain.Enums.ActionType.Insert,
                        Message = status ? "Ok" : "Fail",
                        Status = status
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Insert,
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel> UpdateCartsAsync(List<CartPostDTO> cartsPostDTO)
        {
            try
            {
                // Check if the user is logged in
                var currentUser = await _commonService.GetCurrentUser();
                if (string.IsNullOrEmpty(currentUser))
                {
                    // User is not logged in, manage the cart in the session
                    var sessionCarts = _commonService.Get<List<CartPostDTO>>(GetCartName()) ?? new List<CartPostDTO>();

                    foreach (var cartDTO in cartsPostDTO)
                    {
                        var existingCart = sessionCarts.FirstOrDefault(c => c.ProductId == cartDTO.ProductId);
                        if (existingCart != null)
                        {
                            existingCart.Quantity = cartDTO.Quantity;
                        }
                        else
                        {
                            sessionCarts.Add(cartDTO);
                        }
                    }

                    _commonService.Set(GetCartName(), sessionCarts);

                    return new ResponseModel()
                    {
                        Action = Domain.Enums.ActionType.Update,
                        Message = "Cart updated in session",
                        Status = true
                    };
                }
                else
                {
                    // User is logged in, handle cart in the database
                    var carts = _mapper.Map<List<Cart>>(cartsPostDTO);
                    var customerId = await _commonService.GetCurrentUserId();
                    carts.ForEach(c => c.CustomerId = customerId); // Assuming currentUser is the user ID

                    var result = await _unitOfWork.CartRepository.UpdateCartsAsync(carts);
                    var status = result > 0;

                    return new ResponseModel()
                    {
                        Action = Domain.Enums.ActionType.Update,
                        Message = status ? "Ok" : "Fail",
                        Status = status
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Update,
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        private string GetCartName()
        {
            return _configuration["CartSettings:CartSessionName"];
        }

        public async Task<ResponseModel> DeleteCartsAsync(List<int> cartsId)
        {
            var carts = await _unitOfWork.CartRepository.GetCartsAsync(c => cartsId.Contains(c.Id));
            try
            {
                var result = await _unitOfWork.CartRepository.DeleteCartsAsync((List<Cart>)carts);
                var status = result > 0;
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Delete,
                    Message = status ? "Ok" : "Fail",
                    Status = status
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Delete,
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel> DeleteCartAsync(int productId)
        {
            // Check if the user is authenticated

            var user = await _commonService.GetCurrentUserId();
            if (string.IsNullOrEmpty(user))
            {
                // Handle the case for unauthenticated users (delete from session)
                var carts = _commonService.Get<List<CartPostDTO>>(GetCartName()) ?? new List<CartPostDTO>();

                var cartToDelete = carts.FirstOrDefault(c => c.ProductId == productId);
                if (cartToDelete == null)
                {
                    throw new NotFoundException("Cart not found in session");
                }

                carts.Remove(cartToDelete);
                _commonService.Set(GetCartName(), carts);

                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Delete,
                    Message = "Ok",
                    Status = true
                };
            }

            // Handle the case for authenticated users (delete from database)
            var cart = await _unitOfWork.CartRepository.GetCartAsync(c => c.ProductId == productId);
            if (cart == null)
            {
                throw new NotFoundException("Cart not found in database");
            }
            try
            {
                var result = await _unitOfWork.CartRepository.DeleteCartAsync(cart);
                var status = result > 0;
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Delete,
                    Message = status ? "Ok" : "Fail",
                    Status = status
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Delete,
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<IEnumerable<CartGetDTO>> GetCartItemsByIdsAsync(List<int> checkedItemIds)
        {
            var cartItems = await _unitOfWork.CartRepository.GetCartsAsync(c => checkedItemIds.Contains(c.Id), c => c.Product);
            return _mapper.Map<IEnumerable<CartGetDTO>>(cartItems);
        }
    }
}
