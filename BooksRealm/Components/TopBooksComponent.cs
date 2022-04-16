namespace BooksRealm.Components
{
    using BooksRealm.Models.Books;
    using BooksRealm.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ViewComponent(Name ="TopBooksOf2021")]
    public class TopBooksComponent:ViewComponent
    {
        private readonly IBookService bookService;

        public TopBooksComponent(IBookService bookService)
        {
            this.bookService = bookService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var books =await bookService.GetByCategory<BookInListViewModel>("Top 20 Best Books of 2021");
            return View(books);
        }
    }
}
