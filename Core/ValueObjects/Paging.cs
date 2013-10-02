namespace Svitla.MovieService.Core.ValueObjects
{
    public sealed class Paging
    {
        public Paging(int pagesize, int pagenumber)
        {
            PageSize = pagesize;
            PageNumber = pagenumber;
        }

        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
