using Books.Core.Contracts;
using Books.Core.DataTransferObjects;
using Books.Core.Entities;
using Books.Persistence;
using Books.Wpf.Common;
using Books.Wpf.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Books.Wpf.ViewModels
{
  public class MainWindowViewModel : BaseViewModel
    {

        private string _bookFilterText;
        private BookDto _selectedBook;

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

        public ObservableCollection<BookDto> Books{  get; }

        public MainWindowViewModel() : base(null)
        {
        }

        public ICommand CommandCreateBook { get; set; }
        public ICommand CommandEditBook { get; set; }
        public ICommand CommandDeleteBook { get; set; }

        public MainWindowViewModel(IWindowController windowController) : base(windowController)
        {
            Books = new ObservableCollection<BookDto>();
            BookFilterText = "";
            LoadCommands();
        }

        private void LoadCommands()
        {
            CommandDeleteBook = new RelayCommand( async _ => await DeleteBook(), _ => SelectedBook != null);
            CommandEditBook = new RelayCommand(async _ => await EditBook(), _ => SelectedBook != null);
            CommandCreateBook = new RelayCommand(async _ => await CreateBook(), _ => true);
        }

     /*   public ICommand CmdDeleteBook
        {
            get
            {
                if (_commandDeleteBook == null)
                {
                    _commandDeleteBook = new RelayCommand(
                        execute: _ =>
                        {
                            DeleteBook();
                        },
                        canExecute: _ => SelectedBook != null);

                }
                return _commandDeleteBook;
            }
        }*/

        private async Task DeleteBook()
        {
            await using var uow = new UnitOfWork();
            await uow.Books.DeleteAsync(SelectedBook.Entity);
            await uow.SaveChangesAsync();
            await LoadBooks();
        }

        private async Task EditBook()
        {
            Controller.ShowWindow(new BookEditCreateViewModel(Controller, SelectedBook.Entity));
            await LoadBooks();
        }

        private async Task CreateBook()
        {
            Controller.ShowWindow(new BookEditCreateViewModel(Controller, new Book()));
            await LoadBooks();

        }


        /// <summary>
        /// Lädt die gefilterten Buchdaten
        /// </summary>
        public async Task LoadBooks()
        {
            await using var uow = new UnitOfWork();

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
