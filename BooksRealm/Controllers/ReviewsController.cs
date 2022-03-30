namespace BooksRealm.Controllers
{
    using BooksRealm.Data.Models;
    using BooksRealm.Models.Reviews;
    using BooksRealm.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class ReviewsController:Controller
    {
        private readonly IReviewService reviewService;
        private readonly UserManager<BooksRealmUser> userManager;

        public ReviewsController(IReviewService reviewService, UserManager<BooksRealmUser> userManager)
        {
            this.reviewService = reviewService;
            this.userManager = userManager;
        }
        [HttpPost]
        public IActionResult AddReview(ReviewInputModel input)
        {
            var userId = this.userManager.GetUserId(this.User);
            var review = this.reviewService.AddReview(input.ReviewsText,userId, input.BookId);
            return Redirect($"/Books/ById/{input.BookId}");
        }
    }
}
