using Books.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.Core.Contracts
{
    public interface IBookRepository
    {
        Task AddRangeAsync(IEnumerable<Book> books);
        Task<IEnumerable<Book>> GetFilteredBooksAsync(string bookFilterText, bool orderByPublishers);
        Task DeleteAsync(Book entity);
    }
}