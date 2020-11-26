using Books.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Web.DataTransferObjects
{
    public class BookDto
    {

        public BookDto(Book book)
        {
            Title = book.Title;
            Publishers = book.Publishers;
            Isbn = book.Isbn;
            AuthorNames = book.BookAuthors.Select(ba => ba.Author.Name).ToList();
        }
        public ICollection<String> AuthorNames { get; set; }
        public string Title { get; set; }
        public string Publishers { get; set; }
        public string Isbn { get; set; }
    }
}
