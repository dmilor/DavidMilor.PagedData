namespace DavidMilor.PagedData
{ 
    public class PagedData<T>
    {
        public IEnumerable<T> Items { get;}

        public int Page { get; }
        public int MaximumPage { get; }
        public int PageSize { get; }

        public int FullSetAmount { get; }

        public bool IsFirst => Page == 1;
        public bool IsLast => MaximumPage!=0 && Page == MaximumPage;
        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < MaximumPage;

        public PagedData(IEnumerable<T> items, int page, int pageSize, int maximumPage, int fullSetAmount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            MaximumPage = maximumPage;
            FullSetAmount = fullSetAmount;
        }
    }
}
