using BooksRealm.Models;
using BooksRealm.Services;
using BooksRealm.Models.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BooksRealm.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookService bookService;
        private readonly ICountService countService;

        public HomeController(ILogger<HomeController> logger,IBookService bookService,ICountService countService)
        {
            _logger = logger;
            this.bookService = bookService;
            this.countService = countService;
        }

        public async Task<IActionResult> Index()
        {
            var countsDto =this.countService.GetCounts();
            //// var viewModel2 = AutoMapperConfig.MapperInstance.Map<IndexViewModel>(countsDto);
            //// var viewModel = this.mapper.Map<IndexViewModel>(countsDto);
            var viewModel = new IndexViewModel
            {
                BooksCount = countsDto.BooksCount,
                AuthorsCount = countsDto.AuthorsCount,
                ReviewsCount=countsDto.ReviewsCount,
                GenresCount=countsDto.GenresCount,
                RandomBooks = await this.bookService.GetRandomAsync<IndexPageBooksViewModel>(12),
            };
            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(HttpErrorViewModel errorViewModel)
        {
            if (errorViewModel.StatusCode == 404)
            {
                return this.View(errorViewModel);
            }
            return this.View(
               "Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
            
        }
    }
}