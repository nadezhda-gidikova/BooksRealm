namespace BooksRealm.Areas.Admin.Controllers
{
    using BooksRealm.Models.Authors;
    using BooksRealm.Services;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AuthorsController:AdminController
    {
        private readonly IAuthorService author;

        public AuthorsController(IAuthorService author)
        {
            this.author = author;
        }
        public async Task<IActionResult> All(int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            const int ItemsPerPage = 12;

            var viewModel = new AuthorListViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                ItemsCount = await this.author.GetCountAsync(),
                Authors =await this.author.GetAllInLIstAsync<AuthorViewModel>(id, ItemsPerPage),
            };
            return this.View(viewModel);
        }
        public IActionResult Add()
        {
            return this.View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AuthorInputModel author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var authorId = await this.author.AddAsync(author.Name);
            return RedirectToAction(nameof(All));

        }
        public async Task<IActionResult> Delete(int id)
        {
            var authorId = await this.author.DeleteAsync(id);
            return RedirectToAction(nameof(All));

        }

    }
}
