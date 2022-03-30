namespace BooksRealm.Services
{
    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;
    using BooksRealm.Services.Mapping;
    using BooksRealm.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System;
    using Microsoft.EntityFrameworkCore;
    public class BookService : IBookService
    {
        private readonly BooksRealmDbContext db;
        private readonly IDeletableEntityRepository<Book> booksRepo;
        private readonly IRepository<Author> authorRepo;
        
        public BookService(BooksRealmDbContext db,IDeletableEntityRepository<Book> booksRepo, IRepository<Author> authorRepo)
        {
            this.db = db;
            this.booksRepo = booksRepo;
            this.authorRepo = authorRepo;
        }

        public async Task DeleteAsync(int id)
        {
            var book = this.booksRepo.All().FirstOrDefault(x => x.Id == id);
            this.booksRepo.Delete(book);
            await this.booksRepo.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>(int page, int itemsPerPage = 12)
        {
            var books = this.booksRepo.AllAsNoTracking()
                .OrderByDescending(x => x.DateOfPublish)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .To<T>()
                .ToList();

            return books;


        }

        public T GetById<T>(int id)
        {
            var book = this.booksRepo.AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

            return book;
        }

        public IEnumerable<T> GetByAuthorId<T>(int authorId)
        {
            var query = this.booksRepo.All()
                .Where(x => x.Authors.Any(y => y.AuthorId == authorId))
                .To<T>()
                .ToList();
            return query;

        }
        public IEnumerable<T> GetByAuthorName<T>(string searchTerm, int page, int itemsPerPage = 12)
        {
            var authors = this.authorRepo.All().OrderBy(x => x.Id)
                .Where(c => c.Name.StartsWith(searchTerm) ||
                        c.Name.EndsWith(searchTerm) ||
                        c.Name.Contains(searchTerm))
                .Select(x => x.Id).ToList();


            var query = this.booksRepo.AllAsNoTracking()
                .OrderByDescending(r => r.Rating)
                    .Where(r =>
                               r.Title.StartsWith(searchTerm) ||

                                 r.Title.EndsWith(searchTerm) ||

                                r.Title.Contains(searchTerm)).To<T>().ToList();


            if (query.Count == 0)

            {
                List<T> books = new List<T>();
                foreach (var item in authors)
                {
                    books.AddRange(GetByAuthorId<T>(item));
                   
                }
                return books;
            }
                
        

            return query;
        }
        public IEnumerable<T> GetByTitle<T>(string titleName)
        {
            var query = this.booksRepo.All()
                .Where(x => (!String.IsNullOrEmpty(x.Title) && x.Title.Contains(titleName))
                || x.Title == titleName)
                .To<T>()
                .ToList();

            return query;

        }

        public int GetCount()
        {
            return this.booksRepo.All().Count();
        }

        public IEnumerable<T> GetRandom<T>(int count)
        {
            var books = this.booksRepo.All()
                .OrderBy(x => Guid.NewGuid())
                .Take(count)
                .To<T>().ToList();
            return books;
        }

        public IEnumerable<T> GetByCategory<T>(string categoryName)
        {
            var query = this.booksRepo.All()
                .Distinct()
                .Where(x => x.Genres.Any(y => y.Genre.Name == categoryName))               
                .OrderBy(x => Guid.NewGuid())
                .To<T>()
                .ToList();
            return query;
        }

        
    }
}
