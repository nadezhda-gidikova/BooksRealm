using System;
using System.Linq;
using System.Threading.Tasks;
using BooksRealm.Data.Common;
using BooksRealm.Data.Common.Repositories;
using BooksRealm.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace BooksRealm.Services
{ 
public class RatingsService : IRatingsService
    {
        private readonly IDeletableEntityRepository<Vote> starRatingsRepository;

        public RatingsService(IDeletableEntityRepository<Vote> starRatingsRepository)
        {
            this.starRatingsRepository = starRatingsRepository;
        }

        public async Task VoteAsync(int bookId, string userId, int rating)
        {
            var starRating = await this.starRatingsRepository
                .All()
                .FirstOrDefaultAsync(x => x.BookId == bookId && x.UserId == userId);

            if (starRating != null)
            {
                if (DateTime.UtcNow < starRating.NextDateRate)
                {
                    throw new ArgumentException(ExceptionMessages.AlreadySentVote);
                }

                starRating.Value += rating;
                starRating.NextDateRate = DateTime.UtcNow.AddDays(1);
            }
            else
            {
                starRating = new Vote
                {
                    BookId = bookId,
                    UserId = userId,
                    Value = rating,
                    NextDateRate = DateTime.UtcNow.AddDays(1),
                };

                await this.starRatingsRepository.AddAsync(starRating);
            }

            await this.starRatingsRepository.SaveChangesAsync();
        }

        public async Task<double> GetStarRatingsAsync(int bookId)
        {
            var starRatings = await this.starRatingsRepository
                .All()
                .Where(x => x.BookId == bookId)
                .AverageAsync(x => x.Value);

            return starRatings;
        }

        public async Task<DateTime> GetNextVoteDateAsync(int bookId, string userId)
        {
            var starRating = await this.starRatingsRepository
                .All()
                .FirstAsync(x => x.BookId == bookId && x.UserId == userId);

            return starRating.NextDateRate;
        }
    }
}
