namespace BooksRealm.Services
{
    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;
    using BooksRealm.Models.Authors;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAuthorService
    {
        public IEnumerable<T> GetAllInLIst<T>(int page, int itemsPerPage = 12);
        public ICollection<AuthorViewModel> GetAll();

        public T GetById<T>(int id);

        public Task<int> AddAsync(string name);

        public Task<int> DeleteAsync(int id);

        public int GetCount();


    }
}
