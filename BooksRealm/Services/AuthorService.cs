namespace BooksRealm.Services
{
    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;
    using BooksRealm.Models.Authors;
    using BooksRealm.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AuthorService : IAuthorService
    {
        private readonly IDeletableEntityRepository<Author> authorRepo;

        public AuthorService(IDeletableEntityRepository<Author> authorRepo)
        {
            this.authorRepo = authorRepo;
        }
        public ICollection<AuthorViewModel> GetAll()
        {
            return this.authorRepo.AllAsNoTracking()
                .Select(x => new AuthorViewModel
                {
                    AuthorId = x.Id,
                    AuthorName = x.Name,
                })
                .OrderBy(x => x.AuthorName)
                .ToList();
        }
        public IEnumerable<T> GetAllInLIst<T>(int page, int itemsPerPage = 12)
        {
            var authors = this.authorRepo.AllAsNoTracking()
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .To<T>()
                .ToList();

            return authors;
        }
        public T GetById<T>(int id)
        {
            var author = this.authorRepo
                 .AllAsNoTracking()
                 .Where(x => x.Id == id)
                 .To<T>()
                 .FirstOrDefault();

            return author;
        }
        public async Task<int> AddAsync(string name)
        {
            var author = new Author
            {
                Name = name,
            };
            await this.authorRepo.AddAsync(author);
            await this.authorRepo.SaveChangesAsync();
            return author.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var author = this.authorRepo
                   .AllAsNoTracking()
                   .Where(x => x.Id == id)
                   .FirstOrDefault();
            this.authorRepo.Delete(author);
            await this.authorRepo.SaveChangesAsync();
            return author.Id;
        }
        public int GetCount()
        {
            return this.authorRepo.AllAsNoTracking().Count();
        }
    }
}
