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
    using BooksRealm.Models.Books;
    using System.Globalization;

    public class BookService : IBookService
    {
        private readonly IDeletableEntityRepository<Book> booksRepo;
        private readonly IDeletableEntityRepository<Author> authorRepo;
        private readonly IDeletableEntityRepository<Genre> genreRepo;
        private readonly IRepository<BookGenre> bookGenreRepo;
        private readonly IRepository<AuthorBook> authorBookRepo;

        public BookService(
             IDeletableEntityRepository<Book> booksRepo
            , IDeletableEntityRepository<Author> authorRepo
            , IDeletableEntityRepository<Genre> genreRepo
            , IRepository<BookGenre> bookGenreRepo,
            IRepository<AuthorBook> authorBookRepo)
        {
            this.booksRepo = booksRepo;
            this.authorRepo = authorRepo;
            this.genreRepo = genreRepo;
            this.bookGenreRepo = bookGenreRepo;
            this.authorBookRepo = authorBookRepo;
        }
        public async Task<int> CreateAsync(BookFormModel input)
        {
            var bookNew = new Book
            {
                Title = input.Title,
                Description = input.Description,
                DateOfPublish = input.DateOfPublish,
                CoverUrl = input.CoverUrl,
            };
            await this.booksRepo.AddAsync(bookNew);
            var auth = this.authorRepo
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == input.AuthorId);
            var authorBookData = new AuthorBook
            {
                AuthorId = input.AuthorId,
                BookId = bookNew.Id

            };
            bookNew.Authors.Add(authorBookData);
            await this.authorBookRepo.AddAsync(authorBookData);
            await this.authorBookRepo.SaveChangesAsync();
            var genr = this.genreRepo
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == input.GenreId);
            var genreBook = new BookGenre
            {
                GenreId = input.GenreId,
                BookId = bookNew.Id,
            };
            bookNew.Genres.Add(genreBook);
            await this.bookGenreRepo.AddAsync(genreBook);
            await this.bookGenreRepo.SaveChangesAsync();
            await this.booksRepo.SaveChangesAsync();

            return bookNew.Id;
        }
        public async Task<bool> Edit(
            int id,
            string title,
            string description,
            string coverUrl,
            string dateOfPublish,
             int authorId, 
             int genreId)
        {
            var bookData = this.booksRepo.All()
                
                .FirstOrDefault(x => x.Id == id);

            if (bookData == null)
            {
                return false;
            }
            if (!DateTime.TryParse(dateOfPublish,out DateTime date))
            {
                return false;
            }
            bookData.Title = title;
            bookData.Description = description;
            bookData.CoverUrl = coverUrl;
            bookData.DateOfPublish = date;
           
            if (!this.bookGenreRepo.All().Any(x => x.BookId == bookData.Id && x.GenreId == genreId))
            {
                var genreBook = new BookGenre
                {
                    GenreId = genreId,
                    BookId = bookData.Id,
                };
                
                bookData.Genres.Add(genreBook);
                await this.bookGenreRepo.AddAsync(genreBook);
                await this.bookGenreRepo.SaveChangesAsync();
            }
            if (!this.authorBookRepo.All().Any(x => x.BookId == bookData.Id && x.AuthorId == authorId))
            {
                var authorBook = new AuthorBook
                {
                    AuthorId = authorId,
                    BookId = bookData.Id,
                };
               
                
                bookData.Authors.Add(authorBook);
                await this.authorBookRepo.AddAsync(authorBook);
                await this.authorBookRepo.SaveChangesAsync();
            }
            await this.booksRepo.SaveChangesAsync();
            
            return true;
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
                .OrderByDescending(x => x.Id)
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
        public IEnumerable<T> Search<T>(string searchTerm, int page, int itemsPerPage = 12)
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
