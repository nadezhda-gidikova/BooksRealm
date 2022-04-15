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
        private readonly IDeletableEntityRepository<Book> bookRepository;

        private readonly IBookService bookService;

        public RatingsService(IDeletableEntityRepository<Vote> votesRepository,
            IDeletableEntityRepository<Book> bookRepository)
        {
            this.votesRepository = votesRepository;
            this.bookRepository = bookRepository;
        }

        public async Task VoteAsync(int bookId, string userId, int value)
        {
            var vote = await this.votesRepository
                .All()
                .FirstOrDefaultAsync(x => x.BookId == bookId && x.UserId == userId);

            if (vote != null)
            {
                if (DateTime.UtcNow < vote.NextDateRate)
                {
                    throw new ArgumentException(ExceptionMessages.AlreadySentVote);
                }

                vote.Value = value;
                vote.NextDateRate = DateTime.UtcNow.AddDays(0);
            }
            else
            {
                vote = new Vote
                {
                    BookId = bookId,
                    UserId = userId,
                    Value = value,
                    NextDateRate = DateTime.UtcNow.AddDays(0),
                };

                await this.votesRepository.AddAsync(vote);
            }

            await this.votesRepository.SaveChangesAsync();

        }

        public async Task<double> GetStarRatingsAsync(int bookId)
         {
            var starRatings = await this.votesRepository
                .All()
                .Where(x => x.BookId == bookId)
                .AverageAsync(x => x.Value);
            await SetRating(starRatings, bookId);
            return starRatings;
        }

        public async Task<DateTime> GetNextVoteDateAsync(int bookId, string userId)
        {
            var starRating = await this.votesRepository
                .All()
                .FirstAsync(x => x.BookId == bookId && x.UserId == userId);
          
            return starRating.NextDateRate;
        }
        private async Task SetRating(double starRating,int bookId)
        {
           var book= await bookRepository.All()
                .Where(x=>x.Id==bookId)
                .FirstOrDefaultAsync();

            book.Rating = starRating;
            await bookRepository.SaveChangesAsync();
          
        }
    }
}
