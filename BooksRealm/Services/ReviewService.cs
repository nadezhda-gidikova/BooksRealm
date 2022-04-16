namespace BooksRealm.Services
{
    using BooksRealm.Data.Common;
    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;
    using BooksRealm.Models.Reviews;
    using Microsoft.EntityFrameworkCore;
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
        public async Task<int> AddReview(string content,string userId, int bookId)
        {
            var review = new Review()
            {
                BookId = bookId,
                Content = content,
                UserId = userId,

            };
            bool doesReviewxist = await this.reviewRepo
               .All()
               .AnyAsync(x => x.BookId == review.BookId && x.UserId == userId && x.Content == content);
            if (doesReviewxist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.ReviewAlreadyExists, review.BookId, review.Content));
            }

            await this.reviewRepo.AddAsync(review);
            await this.reviewRepo.SaveChangesAsync();
            return review.Id;
        }
    }
}
