namespace BooksRealm.Services
{
    using System;
    using System.Threading.Tasks;

    public interface IRatingsService
    {
        Task VoteAsync(int bookId, string userId, int rating);

        Task<double> GetStarRatingsAsync(int bookId);

        Task<DateTime> GetNextVoteDateAsync(int bookId, string userId);
    }
}
