using Microsoft.AspNetCore.Mvc;

namespace FoodOnline.Application.DTOs
{

    //request send when using datatable
    public class RequestDatatable
    {
        //number of record display in each page
        [BindProperty(Name = "length")]
        public int PageSize { get; set; }

        // = (PageIndex - 1) * PageSize
        [BindProperty(Name = "start")]
        public int SkipItems { get; set; }

        //search value
        [BindProperty(Name = "search[value]")]
        public string? Keyword { get; set; }
        public int Draw { get; set; }
    }
}
