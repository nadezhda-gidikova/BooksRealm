using System.Threading.Tasks;

namespace BooksRealm.Services
{
    public interface IDataGathererService
    {
        Task ImportBooksAsync(int fromId = 1, int toId = 2);
    }
}