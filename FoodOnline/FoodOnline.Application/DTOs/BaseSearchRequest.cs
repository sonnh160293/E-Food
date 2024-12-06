namespace FoodOnline.Application.DTOs
{


    //base field for a search request with paging
    public class BaseSearchRequest
    {

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
