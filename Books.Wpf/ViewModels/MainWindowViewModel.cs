using Books.Core.Contracts;
using Books.Core.DataTransferObjects;
using Books.Persistence;
using Books.Wpf.Common;
using Books.Wpf.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Wpf.ViewModels
{
  public class MainWindowViewModel : BaseViewModel
    {
        // public ObservableCollection<BookDto> _books;

        private string _bookFilterText;
        private BookDto _selectedBook;
      //  private ObservableCollection<AuthorDto> _books;

        public string BookFilterText
        {
            get => _bookFilterText;
            set
            {
                _bookFilterText = value;
                OnPropertyChanged();
            }
        }

        public BookDto SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<BookDto> Books{  get; }

        public MainWindowViewModel() : base(null)
        {
        }

        public MainWindowViewModel(IWindowController windowController) : base(windowController)
        {
            Books = new ObservableCollection<BookDto>();
            BookFilterText = "";
            LoadCommands();
        }

        private void LoadCommands()
        {
        }



         /// <summary>
        /// Lädt die gefilterten Buchdaten
        /// </summary>
        public async Task LoadBooks()
        {
            await using IUnitOfWork uow = new UnitOfWork();

            var books = await uow.Books.GetFilteredBooksAsync(BookFilterText, false);
            var bookDtos = books.Select(b => new BookDto(b));
            Books.Clear();
            bookDtos.ToList().ForEach(Books.Add);
        }

        public static async Task<BaseViewModel> Create(IWindowController controller)
        {
            var model = new MainWindowViewModel(controller);
            await model.LoadBooks();
            return model;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
  }
}
