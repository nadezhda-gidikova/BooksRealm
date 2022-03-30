using BooksRealm.Models;
using BooksRealm.Services;
using BooksRealm.Models.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

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

        public IActionResult Index()
        {
            var countsDto = this.countService.GetCounts();
            //// var viewModel2 = AutoMapperConfig.MapperInstance.Map<IndexViewModel>(countsDto);
            //// var viewModel = this.mapper.Map<IndexViewModel>(countsDto);
            var viewModel = new IndexViewModel
            {
                BooksCount = countsDto.BooksCount,
                AuthorsCount = countsDto.AuthorsCount,
                ReviewsCount=countsDto.ReviewsCount,
                GenresCount=countsDto.GenresCount,
                RandomBooks = this.bookService.GetRandom<IndexPageBooksViewModel>(12),
            };
            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}