using FoodOnline.Application.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FoodOnline.UI.Ultility
{
    public class CommonDataActionFilter : ActionFilterAttribute
    {

        private readonly ICartService _cartService;
        private readonly ICommonService _commonService;
        public CommonDataActionFilter(ICartService cartService, ICommonService commonService)
        {
            _cartService = cartService;
            _commonService = commonService;
        }

        public override async void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<SkipActionFilterAttribute>().Any())
            {
                return; // Skip filter logic
            }

            var carts = await _cartService.GetCartsAsync(await _commonService.GetCurrentUserId() ?? String.Empty);
            if (carts is not null)
            {
                var controller = context.Controller as Controller;
                controller.ViewData["NumberCart"] = carts.Count();

            }
        }
    }

    public class SiteAreaConvention : IControllerModelConvention
    {
        private readonly ICartService _cartService;
        private readonly ICommonService _commonService;

        public SiteAreaConvention(ICartService cartService, ICommonService commonService)
        {
            _cartService = cartService;
            _commonService = commonService;
        }

        public void Apply(ControllerModel controller)
        {
            var areaAttribute = controller.Attributes.OfType<AreaAttribute>().FirstOrDefault();
            if (string.IsNullOrEmpty(areaAttribute?.RouteValue))
            {
                controller.Filters.Add(new CommonDataActionFilter(_cartService, _commonService));
            }
        }
    }

}
