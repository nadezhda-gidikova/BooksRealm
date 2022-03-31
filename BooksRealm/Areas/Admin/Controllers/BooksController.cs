namespace BooksRealm.Areas.Admin.Controllers
{
    using BooksRealm.Models.Books;
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
                BooksCount = this.books.GetCount(),
                Books = this.books.GetAll<BookInListViewModel>(id, ItemsPerPage),
            };
            return this.View(viewModel);
        }
        public IActionResult Edit(int id)
        {
            

            return View();
        }
        [HttpPost]
        public IActionResult Edit(int id,BookFormModel input)
        {


            return View();
        }
        public IActionResult Add() => View(new BookFormModel
        {
            Authors = this.authors.GetAllAuthors(),
            Genres=this.genres.GetAllAsKeyValuePairs(),

        });
        [HttpPost]
        public async Task<IActionResult> Add(BookFormModel input)
        {
            
            //if (!this.(input.AuthorId))
            //{
            //    this.ModelState.AddModelError(nameof(input.AuthorId), "Author does not exist.");
            //}

            if (!ModelState.IsValid)
            {
                input.Authors = this.authors.GetAllAuthors();
                input.Genres = this.genres.GetAllAsKeyValuePairs();
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

        public IActionResult ChangeVisibility(int id)
        {
            //this.cars.ChangeVisility(id);

            return RedirectToAction(nameof(All));
        }
    }
}
