using Books.Core.Contracts;
using Books.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Persistence
{
  public class BookRepository : IBookRepository
  {
    private readonly ApplicationDbContext _dbContext;

    public BookRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task AddRangeAsync(IEnumerable<Book> books)
    {
             await _dbContext.AddRangeAsync(books);
    }

    public async Task<IEnumerable<Book>> GetFilteredBooksAsync(string bookFilterText, bool orderByPublishers)
    {
            IQueryable<Book> books = _dbContext.Books
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author);
            if (!string.IsNullOrEmpty(bookFilterText))
            {
                books = books.Where(b => b.Title.ToUpper().Contains(bookFilterText));
            }
            books = orderByPublishers ? books.OrderBy(b => b.Publishers) : books.OrderBy(b => b.Title);

            return await books.ToListAsync();

        }

    }

}