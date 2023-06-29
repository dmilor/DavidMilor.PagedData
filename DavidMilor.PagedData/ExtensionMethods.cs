using System.Linq;

namespace DavidMilor.PagedData
{
    public static class ExtensionMethods
    {
        public static PagedData<T> Page<T>(this IOrderedQueryable<T> queryToPage, int page, int pageSize) 
        {
            if(queryToPage==null)
                throw new ArgumentNullException(nameof(queryToPage));
            if(pageSize<1)
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            if (page<1)
                throw new ArgumentOutOfRangeException(nameof(page));

            int fullSetAmount = queryToPage.Count();

            int maximumPage;

            IEnumerable<T>? items;
            
            if(fullSetAmount == 0)
            {
                page = 0;
                maximumPage = 0;
                pageSize = 0;
                items = new List<T>();
            }
            else
            {
                var remainder = fullSetAmount % pageSize;
                maximumPage = ((fullSetAmount - remainder) / pageSize) + (remainder == 0 ? 0 : 1);
                var skipAmount = (page - 1) * pageSize;

                items = queryToPage.Skip(skipAmount).Take(pageSize);
            }


            var result = new PagedData<T>(items, page, pageSize, maximumPage, fullSetAmount);

            return result;
        }
    }
}