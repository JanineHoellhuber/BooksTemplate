using Books.Core.Contracts;
using Books.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Core.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Books.Persistence
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

       public void Add(Author author)
        {
            _dbContext.Add(author);
        }

        public async Task<IEnumerable<AuthorDto>> GetAuthorsOverviewAsync()
        {
            var authors = await _dbContext.Authors
               .Include(a => a.BookAuthors)
               .ThenInclude(ba => ba.Book)
               .Select(a => new
               {
                   a.Id,
                   a.Name,
                   a.BookAuthors.Count,
                   Books = a.BookAuthors
                       .Select(ba => ba.Book)
                       .Distinct()
                       .OrderBy(b => b.Title)
                       .ToList()

               })
               .OrderByDescending(o => o.Count)
               .ThenBy(o => o.Name)
               .Select(o => new AuthorDto
               {
                   Id = o.Id,
                   Author = o.Name,
                   BookCount = o.Count,
                   Books = o.Books
               })
               .ToListAsync();


            return authors;
        }

        public async  Task<AuthorDto> GetById(int value)
        {
            return await _dbContext.Authors
                    .Where(a => a.Id == value)
                    .Select(a => new AuthorDto
                    {
                        Id = a.Id,
                        Author = a.Name,
                        Books = a.BookAuthors.Select(ba => ba.Book).ToList()
                    })
                    .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _dbContext.Authors
               .OrderBy(a => a.Name)
               .ToListAsync();
        }


    }

}