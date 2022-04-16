namespace BooksRealm.Controllers
{
    using BooksRealm.Data.Models;
    using BooksRealm.Models.Reviews;
    using BooksRealm.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class ReviewsController:BaseController
    {
        private readonly IReviewService reviewService;
        private readonly UserManager<BooksRealmUser> userManager;

        public ReviewsController(IReviewService reviewService, UserManager<BooksRealmUser> userManager)
        {
            this.reviewService = reviewService;
            this.userManager = userManager;
        }
      
        [HttpPost]
        public async Task<IActionResult> AddReview(ReviewInputModel input)
        {
            var userId = this.userManager.GetUserId(this.User);
            var review =await  this.reviewService.AddReview(input.Content,userId, input.BookId);
            return Redirect($"/Books/ById/{input.BookId}");
        }
    }
}
