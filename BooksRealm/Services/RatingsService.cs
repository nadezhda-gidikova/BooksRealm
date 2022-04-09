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
        private readonly IDeletableEntityRepository<Vote> votesRepository;

        public RatingsService(IDeletableEntityRepository<Vote> votesRepository)
        {
            this.votesRepository = votesRepository;
        }

        public async Task VoteAsync(int bookId, string userId, int value)
        {
            var starRating = await this.votesRepository
                .All()
                .FirstOrDefaultAsync(x => x.BookId == bookId && x.UserId == userId);

            if (starRating != null)
            {
                if (DateTime.UtcNow < starRating.NextDateRate)
                {
                    throw new ArgumentException(ExceptionMessages.AlreadySentVote);
                }

                starRating.Value += value;
                starRating.NextDateRate = DateTime.UtcNow.AddDays(1);
            }
            else
            {
                starRating = new Vote
                {
                    BookId = bookId,
                    UserId = userId,
                    Value = value,
                    NextDateRate = DateTime.UtcNow.AddDays(1),
                };

                await this.votesRepository.AddAsync(starRating);
            }

            await this.votesRepository.SaveChangesAsync();
        }

        public async Task<double> GetStarRatingsAsync(int bookId)
        {
            var starRatings = await this.votesRepository
                .All()
                .Where(x => x.BookId == bookId)
                .AverageAsync(x => x.Value);

            return starRatings;
        }

        public async Task<DateTime> GetNextVoteDateAsync(int bookId, string userId)
        {
            var starRating = await this.votesRepository
                .All()
                .FirstAsync(x => x.BookId == bookId && x.UserId == userId);

            return starRating.NextDateRate;
        }
    }
}
