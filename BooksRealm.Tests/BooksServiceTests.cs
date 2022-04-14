namespace BooksRealm.Tests
{
    using BooksRealm.Data;
    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;
    using BooksRealm.Data.Repositories;
    using BooksRealm.Models.Authors;
    using BooksRealm.Models.Books;
    using BooksRealm.Models.Genres;
    using BooksRealm.Services;
    using BooksRealm.Services.Mapping;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class BookServiceTests:IDisposable
    {
        private const string TestCoverImageUrl = "https://someurl.com";
        private readonly IBookService bookService;
        private IDeletableEntityRepository<Book> bookRepository;
        private EfDeletableEntityRepository<Genre> genreRepository;
        private EfDeletableEntityRepository<Author> authorRepository;
        private IRepository<AuthorBook> authorBookRepository;
        private IRepository<BookGenre> genreBookRepository;


        private SqliteConnection connection;

        private Book firstBook;
        private Genre firstGenre;
        private Author firstAuthor;
        private BookGenre firstBookGenre;
        private AuthorBook firstAuthorBook;

        public BookServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.bookService = new BookService(this.bookRepository, this.authorRepository, this.genreRepository
                ,genreBookRepository, authorBookRepository);
        }

        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public async Task TestAddingBook()
        {
            this.SeedDatabase();

            BookViewModel bookViewModel;

                var model = new BookFormModel
                {
                    Title = "Spy",
                    DateOfPublish = DateTime.UtcNow,
                    CoverUrl= TestCoverImageUrl,
                    Description = "Test description here",               
                    GenreId = 1,
                    AuthorId = 1,
                };

                var book = await this.bookService.CreateAsync(model);

            var count = await this.bookRepository.All().CountAsync();

            Assert.AreEqual(1, count);
        }
        [Test]
        public async Task TestAddingBookReturnsViewModel()
        {
            this.SeedDatabase();

            BookViewModel bookViewModel;

            var model = new BookFormModel
            {
                Title = "Spy",
                DateOfPublish = DateTime.UtcNow,
                CoverUrl = TestCoverImageUrl,
                Description = "Test description here",
                GenreId = 1,
                AuthorId = 1,
            };

            var bookId = await this.bookService.CreateAsync(model);
            var book = await this.bookRepository.GetByIdWithDeletedAsync(bookId);

            Assert.AreEqual("Spy", book.Title);
            Assert.AreEqual("Test description here", book.Description);
            Assert.AreEqual(TestCoverImageUrl, book.CoverUrl);
         
        }
        [Test]
        public async Task CheckIfGetAllBooksAsyncWorksCorrectly()
        {
           await this.SeedDatabase();
            await this.SeedBooks();

            var result = await this.bookService.GetAllAsync<BookInListViewModel>(1,12);

            var count = result.Count();
            Assert.AreEqual(1, count);
        }
        [Test]
        public async Task CheckIfDeletingBookWorksCorrectly()
        {
            await this.SeedDatabase();
            await this.SeedBooks();
            await this.SeedBookGenres();
            await this.SeedBookAuthors();

            await this.bookService.DeleteAsync(this.firstBook.Id);

            var count = await this.bookRepository.All().CountAsync();

            Assert.AreEqual(0, count);
        }


        public void Dispose()
        {
            this.connection.Close();
            this.connection.Dispose();
        }

        private void InitializeDatabaseAndRepositories()
        {
            this.connection = new SqliteConnection("DataSource=:memory:");
            this.connection.Open();
            var options = new DbContextOptionsBuilder<BooksRealmDbContext>().UseSqlite(this.connection);
            var dbContext = new BooksRealmDbContext(options.Options);

            dbContext.Database.EnsureCreated();

            this.bookRepository = new EfDeletableEntityRepository<Book>(dbContext);
            this.authorRepository = new EfDeletableEntityRepository<Author>(dbContext);
            this.genreBookRepository = new EfRepository<BookGenre>(dbContext);
            this.authorBookRepository = new EfRepository<AuthorBook>(dbContext);
            this.genreRepository = new EfDeletableEntityRepository<Genre>(dbContext);
            
        }

        private void InitializeFields()
        {
            this.firstGenre = new Genre
            {
                Name = "Drama",
            };

         

            this.firstAuthor = new Author
            {
                Name = "Peter Zelinski",
              
            };

            this.firstBook = new Book
            {
                Title = "Secret window",
                DateOfPublish = DateTime.UtcNow,
                Description = "Test description here",
                CoverUrl = TestCoverImageUrl,
            };

            this.firstBookGenre = new BookGenre
            {
                GenreId = 1,
                BookId = 1,
            };

            this.firstAuthorBook = new AuthorBook
            {
                AuthorId = 1,
                BookId = 1,
            };
        }

        private async Task SeedDatabase()
        {
            await this.SeedAuthors();
            await this.SeedGenres();
         
        }

        private async Task SeedAuthors()
        {
            await this.authorRepository.AddAsync(this.firstAuthor);

            await this.authorRepository.SaveChangesAsync();
        }

        private async Task SeedBooks()
        {
            await this.bookRepository.AddAsync(this.firstBook);

            await this.bookRepository.SaveChangesAsync();
        }

        private async Task SeedBookGenres()
        {
            await this.genreBookRepository.AddAsync(this.firstBookGenre);

            await this.genreBookRepository.SaveChangesAsync();
        }

        private async Task SeedBookAuthors()
        {
            await this.authorBookRepository.AddAsync(this.firstAuthorBook);

            await this.authorBookRepository.SaveChangesAsync();
        }

        private async Task SeedGenres()
        {
            await this.genreRepository.AddAsync(this.firstGenre);

            await this.genreRepository.SaveChangesAsync();
        }


        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("BooksRealm"));
    }
}
