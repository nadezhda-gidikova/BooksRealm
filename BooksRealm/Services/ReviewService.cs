namespace BooksRealm.Services
{
    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;
    using BooksRealm.Models.Reviews;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReviewService:IReviewService
    {
        private readonly IDeletableEntityRepository<Review> reviewRepo;

        public ReviewService(IDeletableEntityRepository<Review> reviewRepo)
        {
            this.reviewRepo = reviewRepo;
        }
        public int AddReview(string text,string userId, int bookId)
        {
            var review = new Review()
            {
                BookId = bookId,
                Text = text,
                UserId = userId,

            };
            this.reviewRepo.AddAsync(review);
            this.reviewRepo.SaveChangesAsync();
            return review.Id;
        }
    }
}
