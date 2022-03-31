using BooksRealm.Models.Books;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BooksRealm.Services
{
    public interface IBookService
    {
        public  Task<int> CreateAsync(BookFormModel input);
        public Task DeleteAsync(int id);
        IEnumerable<T> GetAll<T>(int page, int itemsPerPage = 12);
        public T GetById<T>(int id);

        public IEnumerable<T> GetByAuthorId<T>(int authorId);
        public IEnumerable<T> GetByCategory<T>(string categoryName);
        public IEnumerable<T> GetByTitle<T>(string titleName);
        public IEnumerable<T> Search<T>(string authorName, int page, int itemsPerPage = 12);
        public int GetCount();
        public IEnumerable<T> GetRandom<T>(int count);
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
