using BooksRealm.Data.Common.Repositories;
using BooksRealm.Data.Models;
using BooksRealm.Services;
using BooksRealm.Models.Books;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text;
using BooksRealm.Messaging;
using System.Linq;

namespace BooksRealm.Controllers
{
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
       
        public IActionResult ByCategory(string categoryName)
        {
            var books = this.bookService.GetByCategory<BookInListViewModel>(categoryName);
            return this.View(books);
        }
        [HttpPost]
        public IActionResult Search(string search,int id=1)
        {
            if (search == null)
            {
                return this.NotFound();
            }

            var books = this.bookService.Search<BookInListViewModel>(search, 1, 12);
            return this.View(books);
           
        }
        public IActionResult All(int id = 1)
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
                ItemsCount = this.bookService.GetCount(),
                Books = this.bookService.GetAll<BookInListViewModel>(id, ItemsPerPage),
            };
            return this.View(viewModel);
        }
       
        public IActionResult ById(int id)
        {
            var book = this.bookService.GetById<BookViewModel>(id);
            return this.View(book);
        }

        public IActionResult Details(int id)
        {
            var book = this.bookService.GetById<BookViewModel>(id);
            return this.View(book);
        }

        [HttpPost]
        public async Task<IActionResult> SendToEmail(int id)
        {
            var recipe = this.bookService.GetById<BookInListViewModel>(id);
            var html = new StringBuilder();
            html.AppendLine($"<h1>{recipe.Title}</h1>");
            html.AppendLine($"<h3>{recipe.Authors.First()}</h3>");
            html.AppendLine($"<img src=\"{recipe.CoverUrl}\" />");
            await this.emailSender.SendEmailAsync("recepti@recepti.com", "MoiteRecepti", "gerig14198@questza.com", recipe.Title, html.ToString());
            return this.RedirectToAction(nameof(this.ById), new { id });
        }


    }
}
