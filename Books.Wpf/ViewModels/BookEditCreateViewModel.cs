using Books.Core.Entities;
using Books.Core.Validations;
using Books.Persistence;
using Books.Wpf.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Books.Wpf.ViewModels
{
  public class BookEditCreateViewModel : BaseViewModel
  {
        private Book _book;
        private string _title;
        private string _bookPublishers;
        private string _isbn;
        private Author _selectedAuthorToAdd;

        public ICommand CommandSaveBook { get; set; }
        public ICommand CommandAddAuthor { get; set; }
        public ICommand CommandRemoveAuthor { get; set; }

        public ObservableCollection<string> AllPublishers { get; set; }
        public ObservableCollection<Author> SelectedAuthors { get; set; }
        public ObservableCollection<Author> AvailableAuthors { get; set; }
        public string WindowTitle { get; set; }

        public Book Book
        {
            get => _book;
            set
            {
                _book = value;
                OnPropertyChanged(nameof(Book));
            }
        }
        [Required(ErrorMessage = "Titel muss angegeben werden")]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
                ValidateViewModelProperties();
            }
        }

        public string BookPublishers
        {
            get => _bookPublishers;
            set
            {
                _bookPublishers = value;
                OnPropertyChanged(nameof(_bookPublishers));
                ValidateViewModelProperties();
            }
        }
        [IsbnValidation]
        public string Isbn
        {
            get => _isbn;
            set
            {
                _isbn = value;
                OnPropertyChanged();
                ValidateViewModelProperties();
            }
        }

        public Author SelectedAuthorToAdd
        {
            get => _selectedAuthorToAdd;
            set
            {
                _selectedAuthorToAdd = value;
                OnPropertyChanged();
               
            }
        }

        public BookEditCreateViewModel() : base(null)
        {
          
        }

    public BookEditCreateViewModel(IWindowController windowController, Book book) : base(windowController)
    {
            _book = book;
            SelectedAuthors = new ObservableCollection<Author>();
            WindowTitle = _book.Id != 0 ? "Buch Bearbeiten" : "Buch anlegen";
            LoadCommands();
    }

    public static async Task<BaseViewModel> Create(WindowController controller, Book book)
    {
      var model = new BookEditCreateViewModel(controller, book);
      await model.LoadData();
      return model;
    }

    private Task LoadData()
    {
      throw new NotImplementedException();
    }

    private void LoadCommands()
    {
            CommandSaveBook = new RelayCommand(async c => await SaveBook(), c => !HasErrors);
            CommandAddAuthor = new RelayCommand(c => AddAuthor(), c => SelectedAuthorToAdd != null);
            CommandRemoveAuthor = new RelayCommand(c => RemoveAuthor(), c => SelectedAuthorToRemove != null);

     }

        public Author SelectedAuthorToRemove
        {
            get => _selectedAuthorToAdd;
            set { _selectedAuthorToAdd = value;
                OnPropertyChanged(); 
                 }
        }
        private void AddAuthor()
        {
            var author = SelectedAuthorToAdd;
            SelectedAuthors.Add(author);
            AvailableAuthors.Remove(author);
            SelectedAuthorToAdd = null;
        }



        /// <summary>
        /// Autor wird aus den Buchautoren entfernt
        /// </summary>
        private void RemoveAuthor()
        {
            var author = SelectedAuthorToRemove;
            SelectedAuthors.Remove(author);
            AvailableAuthors.Add(author);
            SelectedAuthorToRemove = null;
        }

        public async Task SaveBook()
        {
            await using var uow = new UnitOfWork();
            if (_book.Id != 0)
                _book = await uow.Books.GetByIdWithAuthorsAsync(_book.Id);
            _book.Title = _title;
            _book.Isbn = _isbn;
            _book.Publishers = BookPublishers;
            _book.BookAuthors.Clear();
            foreach (var author in SelectedAuthors)  
            {
               
                _book.BookAuthors.Add(new BookAuthor { AuthorId = author.Id, BookId = _book.Id });
            }

            if (_book.Id == 0)
            {
                uow.Books.Update(Book);
                await uow.SaveChangesAsync();
            }

            try
            {
                await uow.SaveChangesAsync();
            }
            catch (ValidationException e)
            {
                DbError = e.Message;
                return;
            }
            Controller.CloseWindow(this);
        }


    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      yield return ValidationResult.Success;
    }

  }
}
