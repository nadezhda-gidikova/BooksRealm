namespace BooksRealm.Areas.Admin.Controllers
{
    using BooksRealm.Models.Genres;
    using BooksRealm.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class GenresController:AdminController
    {
        private readonly IGenreService genre;

        public GenresController(IGenreService genre)
        {
            this.genre = genre;
        }
        public async Task<IActionResult> All(int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            const int ItemsPerPage = 12;

            var viewModel = new GenreListViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                ItemsCount = await this.genre.GetCountAsync(),
                Genres = await this.genre.GetAllInLIstAsync<GenreViewModel>(id, ItemsPerPage),
            };
            return this.View(viewModel);
        }
        public IActionResult Add()
        {
            return this.View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(GenreInputModel genre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var genreId = await this.genre.AddAsync(genre.Name);
            return RedirectToAction(nameof(All));

        }
        public async Task<IActionResult> Delete(int id)
        {
            var genreId = await this.genre.DeleteAsync(id);
            return RedirectToAction(nameof(All));

        }

    }
}

