namespace BooksRealm.Controllers
{
    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;
    using BooksRealm.Services;
    using BooksRealm.Models.Books;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using System.Text;
    using BooksRealm.Messaging;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;

    public class BooksController:Controller
    {
        private readonly IBookService bookService;
        private readonly IDeletableEntityRepository<Book> bookRepository;
        private readonly IEmailSender emailSender;

        public BooksController(IBookService bookService, IDeletableEntityRepository<Book>bookRepository,IEmailSender emailSender )
        {
            this.bookService = bookService;
            this.bookRepository = bookRepository;
            this.emailSender = emailSender;
        }
       
        public async Task<IActionResult> ByCategory(string categoryName)
        {
            var books =await this.bookService.GetByCategory<BookInListViewModel>(categoryName);
            //ViewBag.Category = categoryName;
            return this.View(books);
        }
        [HttpPost]
        public async Task<IActionResult> Search(string search,int id=1)
        {
            
            if (search == null)
            {
                return this.NotFound();
            }

            var books =await this.bookService.Search<BookInListViewModel>(search, 1, 12);
            return this.View(books);
           
        }
        public async Task<IActionResult> All(int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            const int ItemsPerPage = 12;
   
            var viewModel = new BookListViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                ItemsCount = await this.bookService.GetCountAsync(),
                Books = await this.bookService.GetAllAsync<BookInListViewModel>(id, ItemsPerPage),
            };
            return this.View(viewModel);
        }
       
        public async Task<IActionResult> ById(int id)
        {
            var book =await this.bookService.GetByIdAsync<BookViewModel>(id);
            return this.View(book);
        }

        public IActionResult Details(int id)
        {
            var book = this.bookService.GetByIdAsync<BookViewModel>(id);
            return this.View(book);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendToEmail(int id)
        {
            var book = await this.bookService.GetByIdAsync<BookInListViewModel>(id);
            var html = new StringBuilder();
            html.AppendLine($"<h1>{book.Title}</h1>");
            html.AppendLine($"<h3>{book.Authors.First()}</h3>");
            html.AppendLine($"<img src=\"{book.CoverUrl}\" />");
            await this.emailSender.SendEmailAsync("info@booksrealm.com", "Booksrealm", "nadezhda_gidikova@yahoo.com", book.Title, html.ToString());
            return this.RedirectToAction(nameof(this.ById), new { id });
        }


    }
}
