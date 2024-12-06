using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Domain.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodOnline.Application.DTOs.ViewModel
{
    public class OrderIndexViewModel
    {
        public PaginatedList<OrderGetDTO> Orders { get; set; }
        public OrderSeachRequestAdmin SearchRequest { get; set; }
        public IEnumerable<OrderStatusDropDownDTO> OrderStatuses { get; set; }
        public IEnumerable<SelectListItem> PaidOptions { get; set; }
        public IEnumerable<SelectListItem> ConfirmOptions { get; set; }
    }

}
