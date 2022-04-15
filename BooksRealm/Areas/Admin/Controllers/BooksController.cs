namespace BooksRealm.Areas.Admin.Controllers
{
    using BooksRealm.Infrastructure;
    using BooksRealm.Models.Authors;
    using BooksRealm.Models.Books;
    using BooksRealm.Models.Genres;
    using BooksRealm.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;

    public class BooksController:AdminController
    {
        private readonly IBookService books;
        private readonly IGenreService genres;
        private readonly IAuthorService authors;

        
        public BooksController(IBookService books,IGenreService genres,IAuthorService authors)
        {
            this.books = books;
            this.genres = genres;
            this.authors = authors;
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
                ItemsCount =await this.books.GetCountAsync(),
                Books = await this.books.GetAllAsync<BookInListViewModel>(id, ItemsPerPage),
            };
            return this.View(viewModel);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var book = await this.books.GetByIdAsync<BookViewModel>(id);

            var newBook = new BookFormModel
            {
                Title = book.Title,
                Description = book.Description,
                CoverUrl = book.CoverUrl,
                DateOfPublish = book.DateOfPublish,
            };
            
            newBook.Authors =await this.authors.GetAllAsync<AuthorViewModel>();
            newBook.Genres =await this.genres.GetAllAsync<GenreViewModel>();

            return View(newBook);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id,BookFormModel input)
        {
            if (!ModelState.IsValid)
            {
                input.Authors =await this.authors.GetAllAsync<AuthorViewModel>();
                input.Genres =await this.genres.GetAllAsync<GenreViewModel>();
                return View(input);

            }
            var edited = await this.books.Edit(
                id,
                input.Title,
                input.Description,
                input.CoverUrl,
                input.DateOfPublish.ToString("Y"),
                input.AuthorId,
                input.GenreId);

            if (!edited)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
            
        }
        public async Task<IActionResult> Add() => View(new BookFormModel
        {
            Authors = await this .authors.GetAllAsync<AuthorViewModel>(),
            Genres = await this.genres.GetAllAsync<GenreViewModel>(),

        });
        [HttpPost]
        public async Task<IActionResult> Add(BookFormModel input)
        {

            //if (!this.authors.Any(input.AuthorId))
            //{
            //    this.ModelState.AddModelError(nameof(input.AuthorId), "Author does not exist.");
            //}

            if (!ModelState.IsValid)
            {
                input.Authors = await this.authors.GetAllAsync<AuthorViewModel>();
                input.Genres = await this.genres.GetAllAsync<GenreViewModel>();
                return View(input);
            }

            var newBook = await this.books.CreateAsync(input);
            return RedirectToAction(nameof(All));
        }
        public async Task<IActionResult> Delete(int id)
        {
           await books.DeleteAsync(id);

            return RedirectToAction(nameof(this.All));
        }
    }
}
