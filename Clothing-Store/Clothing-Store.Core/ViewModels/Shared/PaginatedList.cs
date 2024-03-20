namespace Clothing_Store.Core.ViewModels.Shared
{
    using Microsoft.EntityFrameworkCore;
    public class PaginatedList<T> : List<T>
    {
        public PaginatedList(List<T> items, int itemsCount, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(itemsCount / (double)pageSize);

            AddRange(items);
        }

        public int PageNumber { get; private set; }

        public int TotalPages { get; private set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
