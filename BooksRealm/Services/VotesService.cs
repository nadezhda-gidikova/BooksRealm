namespace BooksRealm.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;

    public class VotesService : IVotesService
    {
        private readonly IRepository<Vote> votesRepository;
        private readonly IDeletableEntityRepository<Book> bookRepository;

        public VotesService(IRepository<Vote> votesRepository,IDeletableEntityRepository<Book>bookRepository)
        {
            this.votesRepository = votesRepository;
            this.bookRepository = bookRepository;
        }

        public double GetAverageVotes(int bookId)
        {
            return this.votesRepository.All()
                .Where(x => x.BookId == bookId)
                .Average(x => x.Value);
        }

        public async Task SetVoteAsync(int bookId, string userId, int value)
        {
            var vote = this.votesRepository.All()
                .FirstOrDefault(x => x.BookId == bookId && x.UserId == userId);
            if (vote == null)
            {
                vote = new Vote
                {
                    BookId = bookId,
                    UserId = userId,
                };

                await this.votesRepository.AddAsync(vote);
            }

            vote.Value = value;
            var book = bookRepository.AllAsNoTracking()
                 .FirstOrDefault(x => x.Id == bookId);

              book.Votes.Add(vote);
            await this.votesRepository.SaveChangesAsync();
            var rating= GetAverageVotes(bookId);
            book.Rating = rating;
            await this.bookRepository.SaveChangesAsync();
           

        }
    }
}
