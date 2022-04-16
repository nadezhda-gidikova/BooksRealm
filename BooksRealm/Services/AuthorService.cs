namespace BooksRealm.Services
{
    using BooksRealm.Data.Common;
    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;
    using BooksRealm.Models.Authors;
    using BooksRealm.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
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
        public async Task<ICollection<T>> GetAllAsync<T>()
        {
            return await this.authorRepo.All()
                .OrderBy(x => x.Name)
                .To<T>()
                .ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllInLIstAsync<T>(int page, int itemsPerPage = 12)
        {
            var authors =await this.authorRepo.All()
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .To<T>()
                .ToListAsync();

            return authors;
        }
        public async Task<T> GetByIdAsync<T>(int id)
        {
            var author =await this.authorRepo
                 .All()
                 .Where(x => x.Id == id)
                 .To<T>()
                 .FirstOrDefaultAsync();
            if (author==null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.AuthorNotFound, id));
            }
            return author;
        }
        public async Task<int> AddAsync(string name)
        {
            var author = new Author
            {
                Name = name,
            };
            bool doesAutorExist = await this.authorRepo
                .All()
                .AnyAsync(x => x.Name == author.Name);
            if (doesAutorExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.AuthorAlreadyExists, author.Name));
            }
            await this.authorRepo.AddAsync(author);
            await this.authorRepo.SaveChangesAsync();
            return author.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var author = this.authorRepo
                   .All()
                   .Where(x => x.Id == id)
                   .FirstOrDefault();
            if (author == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.AuthorNotFound, id));
            }
            this.authorRepo.Delete(author);
            await this.authorRepo.SaveChangesAsync();
            return author.Id;
        }
        public async Task<int> GetCountAsync()
        {
            return await this.authorRepo.AllAsNoTracking().CountAsync();
        }
    }
}
