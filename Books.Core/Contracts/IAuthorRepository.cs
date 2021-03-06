﻿using Books.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Books.Core.DataTransferObjects;

namespace Books.Core.Contracts
{
    public interface IAuthorRepository
    {
        void Add(Author author);
        Task<IEnumerable<AuthorDto>> GetAuthorsOverviewAsync();
        Task<AuthorDto> GetById(int value);
        Task<IEnumerable<Author>> GetAllAsync();
    }
}