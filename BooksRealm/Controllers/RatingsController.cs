namespace BooksRealm.Controllers
{
    using System;
    using System.Threading.Tasks;
    using BooksRealm.Data.Common;
    using BooksRealm.Data.Models;
    using BooksRealm.Models.Ratings;

    using BooksRealm.Services;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingsService ratingsService;
        private readonly UserManager<BooksRealmUser> userManager;

        public RatingsController(IRatingsService ratingsService, UserManager<BooksRealmUser> userManager)
        {
            this.ratingsService = ratingsService;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<StarRatingResponseModel>> Post(RatingInputModel input)
        {
            var userId = this.userManager.GetUserId(this.User);
            var starRatingResponseModel = new StarRatingResponseModel();

            if (userId == null)
            {
                starRatingResponseModel.AuthenticateErrorMessage = ExceptionMessages.AuthenticatedErrorMessage;
                starRatingResponseModel.StarRatingsSum = await this.ratingsService.GetStarRatingsAsync(input.BookId);

                return starRatingResponseModel;
            }

            try
            {
                await this.ratingsService.VoteAsync(input.BookId, userId, input.Value);
            }
            catch (ArgumentException ex)
            {
                starRatingResponseModel.ErrorMessage = ex.Message;
                return starRatingResponseModel;
            }
            finally
            {
                starRatingResponseModel.StarRatingsSum = await this.ratingsService.GetStarRatingsAsync(input.BookId);
                starRatingResponseModel.NextVoteDate = await this.ratingsService.GetNextVoteDateAsync(input.BookId, userId);
            }

            return starRatingResponseModel;
        }
    }
}
