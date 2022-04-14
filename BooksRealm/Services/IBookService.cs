using BooksRealm.Models.Books;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BooksRealm.Services
{
    public interface IBookService
    {
        public Task<int> CreateAsync(BookFormModel input);
        public Task DeleteAsync(int id);
        public Task<IEnumerable<T>> GetAllAsync<T>(int page, int itemsPerPage = 12);
        public Task<T> GetByIdAsync<T>(int id);
        public Task<IEnumerable<T>> GetByAuthorIdAsync<T>(int authorId);
        public Task<IEnumerable<T>> GetByCategory<T>(string categoryName);
        public Task<IEnumerable<T>> GetByTitle<T>(string titleName);
        public Task<IEnumerable<T>> Search<T>(string searchTerm, int page, int itemsPerPage = 12);
        public Task<int> GetCountAsync();
        public Task<IEnumerable<T>> GetRandomAsync<T>(int count);
        public Task<bool> Edit(
            int id,
            string title,
            string description,
            string coverUrl,
            string dateOfPublish,
            int authorId,
            int genreId
            );


    }
}
