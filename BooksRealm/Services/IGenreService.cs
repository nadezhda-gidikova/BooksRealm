namespace BooksRealm.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGenreService
   {
        public Task<ICollection<T>> GetAllAsync<T>();
        public Task<IEnumerable<T>> GetAllInLIstAsync<T>(int page, int itemsPerPage = 12);
        public Task<T> GetByIdAsync<T>(int id);
        public Task<int> AddAsync(string name);
        public Task<int> DeleteAsync(int id);
        public Task<int> GetCountAsync();
    }
}
