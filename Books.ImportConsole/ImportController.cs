using Books.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Books.ImportConsole
{
  public static class ImportController
  {
    public static IEnumerable<Book> ReadBooksFromCsv()
    {
            string[][] matrix = MyFile.ReadStringMatrixFromCsv("books.csv", false);
            var authors = matrix
              .SelectMany(line => line[0].Split('~'))
              .Distinct()
              .Select(text => new Author
              {
                  Name = text
              })
              .OrderBy(a => a.Name)
              .ToList();
            var books = matrix
                .Select(line => new Book()
                {
                    Title = line[1],
                    Publishers = line[2],
                    Isbn = line[3]
                })
                .ToList();
            var bookAuthors = matrix
                    .SelectMany(line => line[0].Split('~'), (line, authorName) => new BookAuthor  //line an der 0 wird gesplittet und ist der áuthorname zudem wird line mitgegeben damit an der stelle 3 die isbn verglichen werden kann
                    {
                        Author = authors.Single(a => a.Name == authorName),
                        Book = books.Single(b => b.Isbn == line[3])
                    })
                .ToList();

            return books;

    }
  }
}
