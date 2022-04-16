namespace BooksRealm.Services
{
    using System.Threading.Tasks;

    public interface IDataGathererService
    {
        Task ImportBooksAsync(int fromId = 1, int toId = 2);
    }
}