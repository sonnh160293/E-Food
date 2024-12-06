namespace FoodOnline.Domain.Common
{

    //do the paging
    public class PaginatedList<T> : List<T> where T : class
    {


        public List<T> Items { get; private set; }


        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        private PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageSize = pageSize;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public static PaginatedList<T> Create(List<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count; // Count items directly from the list
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(); // Apply paging

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

    }
}
