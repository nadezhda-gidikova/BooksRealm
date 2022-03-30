namespace BooksRealm.Services
{
    using System.Threading.Tasks;

    public interface IVotesService
    {
        Task SetVoteAsync(int bookId, string userId, int value);

        double GetAverageVotes(int bookId);
    }
}
