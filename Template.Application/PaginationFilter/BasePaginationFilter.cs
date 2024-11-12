namespace Template.Application.PaginationFilter
{
    public class BasePaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public BasePaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public BasePaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}
