using Microsoft.AspNetCore.Mvc;
using BooksRealm.Services;
using System.Threading.Tasks;

namespace BooksRealm.Controllers
{
    public class GatherBooksController:Controller
    {
        private readonly IDataGathererService service;
        private readonly IBookService bookService;

        public GatherBooksController(IDataGathererService service, IBookService bookService)
        {
            this.service = service;
            this.bookService = bookService;
        }

        public IActionResult Index()
        {
            return this.View();
        }
        public async Task<IActionResult> Add()
        {
            await this.service.ImportBooksAsync(400,1500);
            //this.bookService.AddRating();
            return Redirect("/GatherBooks/Index");
        }
    }
}
