using Books.Core.Entities;
using Books.Core.Validations;
using Books.Wpf.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Books.Wpf.DataTransferObjects
{
   public class BookDto : NotifyPropertyChanged
    {
        private int _id;
        private string _title;
        private string _authorNames;
        private string _publishers;
        private string _isbn;

        public BookDto(Book book)
        {
            Entity = book;
            Id = book.Id;
            Title = book.Title;
            Isbn = book.Isbn;
            Publishers = book.Publishers;
        }

        public BookDto() : this(new Book())
        {

        }

        public Book Entity { get; }
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string AuthorNames
        {
            get => _authorNames;
            set { _authorNames = value; OnPropertyChanged(); }
        }

        public string Publishers
        {
            get => _publishers;
            set { _publishers = value; OnPropertyChanged(); }
        }

        public string Isbn
        {
            get => _isbn;
            set { _isbn = value; OnPropertyChanged(); }
        }



    }
}
