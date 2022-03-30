using BooksRealm.Data.Common.Repositories;
using BooksRealm.Data.Models;
using BooksRealm.Services;
using BooksRealm.Models.Books;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BooksRealm.Controllers
{
    public class BooksController:Controller
    {
        private readonly IBookService bookService;
        private readonly IDeletableEntityRepository<Book> bookRepository;

        public BooksController(IBookService bookService, IDeletableEntityRepository<Book>bookRepository )
        {
            this.bookService = bookService;
            this.bookRepository = bookRepository;
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

            //const int ItemsPerPage = 12;
            ////var books = this.bookService.GetByAuthorName<BookInListViewModel>(search,id,ItemsPerPage);
            ////if (books == null)
            ////{
            ////    //books = this.bookService.GetByTitle<BookInListViewModel>(search);
            ////}
            //var viewModel = new BookListViewModel
            //{
            //    ItemsPerPage = ItemsPerPage,
            //    PageNumber = id,
            //    BooksCount = this.bookService.GetCount(),
            //    Books =this.bookService.GetByAuthorName<BookInListViewModel>(search, id, ItemsPerPage),
            //};
            var books = this.bookService.GetByAuthorName<BookInListViewModel>(search, 1, 12);
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
                BooksCount = this.bookService.GetCount(),
                Books = this.bookService.GetAll<BookInListViewModel>(id, ItemsPerPage),
            };
            return this.View(viewModel);
        }
       
        public IActionResult ById(int id)
        {
            var book = this.bookService.GetById<BookViewModel>(id);
            return this.View(book);
        }

        [HttpPost]
        //[Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(int id)
        {
            await this.bookService.DeleteAsync(id);
            return this.RedirectToAction(nameof(this.All));
        }
        public IActionResult Details(int id)
        {
            var book = this.bookService.GetById<BookViewModel>(id);
            return this.View(book);
        }

        //[HttpPost]
        //public async Task<IActionResult> SendToEmail(string id)
        //{
        //    var recipe = this.bookService.GetById<BookInListViewModel>(id);
        //    var html = new StringBuilder();
        //    html.AppendLine($"<h1>{recipe.Title}</h1>");
        //    html.AppendLine($"<h3>{recipe.Authors}</h3>");
        //    html.AppendLine($"<img src=\"{recipe.CoverUrl}\" />");
        //    //await this.emailSender.SendEmailAsync("recepti@recepti.com", "MoiteRecepti", "gerig14198@questza.com", recipe.Name, html.ToString());
        //    return this.RedirectToAction(nameof(this.ById), new { id });
        //}


    }
}
