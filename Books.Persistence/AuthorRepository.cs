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

        public async Task<AuthorDto[]> GetAuthorsOverviewAsync()
        {
            return await _dbContext.Authors
          .Select(a => new AuthorDto()
          {
              Author = a.Name,
              BookCount = a.BookAuthors.Count(),
              PublisherName = a.BookAuthors.Select(b => b.Book.Publishers)
          })
          .OrderBy(dto => dto.Author)
          .ToArrayAsync();
        }

        public async  Task<AuthorDto> GetById(int value)
        {
            return await _dbContext.Authors
                .Where(a => a.Id == value)
                .Select(a => new AuthorDto
                {
                    Id = a.Id,
                    Author = a.Name,
                    Books = a.BookAuthors.Select(b => b.Book)
                }).SingleOrDefaultAsync();
        }



    }

}