namespace BooksRealm.Services
{
    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;
    using BooksRealm.Services.Mapping;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System;
    using Microsoft.EntityFrameworkCore;
    using BooksRealm.Models.Books;
    using BooksRealm.Data.Common;

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
            if (booksRepo.All().Any(x=>x.Title==bookNew.Title))
            {
                throw new ArgumentException(
                   string.Format(ExceptionMessages.BookAlreadyExists,input.Title));
            }
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
            var bookData = await this.booksRepo.All()
                
                .FirstOrDefaultAsync(x => x.Id == id);

            if (bookData == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.BookNotFound, id));
            }
            if (!DateTime.TryParse(dateOfPublish,out DateTime date))
            {
                throw new InvalidOperationException(
                    string.Format(ExceptionMessages.NotValidDate));
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
            if (book == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.BookNotFound, id));
            }
            this.booksRepo.Delete(book);
            await this.booksRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(int page, int itemsPerPage = 12)
        {
            var books =await this.booksRepo.AllAsNoTracking()
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .To<T>()
                .ToListAsync();

            return books;
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            var book =await this.booksRepo.All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();
            if (book == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.BookNotFound, id));
            }
            return book;
        }

        public async Task<IEnumerable<T>> GetByAuthorIdAsync<T>(int authorId)
        {
            var query =await this.booksRepo.All()
                .Where(x => x.Authors.Any(y => y.AuthorId == authorId))
                .To<T>()
                .ToListAsync();
            
            return query;

        }
        public async Task<IEnumerable<T>> Search<T>(string searchTerm, int page, int itemsPerPage = 12)
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
                    books.AddRange(await GetByAuthorIdAsync<T>(item));

                }
                return books;
            }
            return query;
        }
        public async Task<T> GetByTitle<T>(string titleName)
        {
            var query =await this.booksRepo.All()
                .Where(x => (!String.IsNullOrEmpty(x.Title) && x.Title.Contains(titleName))
                || x.Title == titleName)
                .To<T>()
                .FirstOrDefaultAsync();

            return query;

        }

        public async Task<int> GetCountAsync()
        {
            return await this.booksRepo.All().CountAsync();
        }

        public async Task<IEnumerable<T>> GetRandomAsync<T>(int count)
        {
            var books = await this.booksRepo.All()
                .OrderBy(x => Guid.NewGuid())
                .Take(count)
                .To<T>().ToListAsync();
            return books;
        }

        public async Task<IEnumerable<T>> GetByCategory<T>(string categoryName)
        {
            var query =await this.booksRepo.All() 
                .Where(x => x.Genres.Any(y => y.Genre.Name == categoryName))
                .OrderBy(x => x.DateOfPublish)
                .Distinct()
                .To<T>()
                .ToListAsync();
            return query;
        }


    }
}
