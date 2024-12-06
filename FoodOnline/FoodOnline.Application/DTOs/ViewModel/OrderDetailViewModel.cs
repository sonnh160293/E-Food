using FoodOnline.Application.DTOs.GetDTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodOnline.Application.DTOs.ViewModel
{
    public class OrderDetailViewModel
    {
        public OrderGetDTO Order { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public int SelectedStatusId { get; set; }
        public IEnumerable<OrderDetailGetDTO> OrderDetails { get; set; }
    }
}
