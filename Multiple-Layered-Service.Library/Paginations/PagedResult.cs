namespace Multiple_Layered_Service.Library.Paginations
{
    public class PagedResult<T>
    {
        public PagedResult(IEnumerable<T> data, int totalCount, int page, int size)
        {
            Data = data;
            TotalCount = totalCount;
            Page = page;
            Size = size;
        }

        public IEnumerable<T> Data { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
