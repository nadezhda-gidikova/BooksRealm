namespace BooksRealmTests
{
    using BooksRealm.Data;
    using BooksRealm.Data.Common;
    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;
    using BooksRealm.Data.Repositories;
    using BooksRealm.Models.Books;
    using BooksRealm.Services;
    using BooksRealm.Services.Mapping;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Xunit;

    public class BookServiceTests : IDisposable, IClassFixture<Configuration>
    {
        private const string TestCoverImageUrl = "https://someurl.com";
        private readonly IBookService bookService;
        private EfDeletableEntityRepository<Book> bookRepository;
        private EfDeletableEntityRepository<Genre> genreRepository;
        private EfDeletableEntityRepository<Author> authorRepository;
        private IRepository<AuthorBook> authorBookRepository;
        private IRepository<BookGenre> genreBookRepository;


        private SqliteConnection connection;

        private Book secondBook;
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
                , genreBookRepository, authorBookRepository);
        }


        [Fact]
        public async Task TestAddingBook()
        {
            await this.SeedDatabase();

            var model = new BookFormModel
            {
                Title = "Spy",
                DateOfPublish = DateTime.UtcNow,
                CoverUrl = TestCoverImageUrl,
                Description = "Test description here",
                GenreId = 1,
                AuthorId = 1,
            };

            var book = await this.bookService.CreateAsync(model);

            var count = await this.bookRepository.All().CountAsync();

            Assert.Equal(1, count);
            await this.bookService.DeleteAsync(book);
        }
        [Fact]
        public async Task TestAddingBookReturnsViewModel()
        {
            await SeedDatabase();

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

            Assert.Equal("Spy", book.Title);
            Assert.Equal("Test description here", book.Description);
            Assert.Equal(TestCoverImageUrl, book.CoverUrl);
        }

        [Fact]
        public async Task TestEditingBook()
        {
            await SeedDatabase();

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

            var edited = await bookService.Edit(bookId, "Renamed", "Editted description", TestCoverImageUrl, "11-10-22", 1, 1);

            Assert.Equal("Renamed", book.Title);
            Assert.Equal("Editted description", book.Description);
            Assert.Equal(TestCoverImageUrl, book.CoverUrl);
        }
        [Fact]
        public async Task CheckIfGetAllBooksAsyncWorksCorrectly()
        {
            await this.SeedBooks();

            var result = await this.bookService.GetAllAsync<BookInListViewModel>(1, 12);

            var count = result.Count();
            Assert.Equal(1, count);
        }
        [Fact]
        public async Task CheckIfDeletingBookWorksCorrectly()
        {
            await this.SeedBooks();

            await this.bookService.DeleteAsync(this.secondBook.Id);

            var count = await this.bookRepository.All().CountAsync();

            Assert.Equal(0, count);
        }
        [Fact]
        public async Task CheckIfDeletingBookThrowsExceptionWithUnexistingBook()
        {
            await this.SeedBooks();

            var exception =
                Assert
                .ThrowsAsync<NullReferenceException>(() => this.bookService.DeleteAsync(3));
            //Assert.Equal(string.Format(ExceptionMessages.BookNotFound, 3), exception);

        }
        [Fact]
        public async Task CheckIfGetByIdReturnsRightBook()
        {
            await this.SeedBooks();

            var third = new Book
            {
                Title = "Right book",
                DateOfPublish = DateTime.UtcNow,
                Description = "Test description second book",
                CoverUrl = TestCoverImageUrl,

            };

            await this.bookRepository.AddAsync(third);
            await this.bookRepository.SaveChangesAsync();
            var book = await this.bookRepository.GetByIdWithDeletedAsync(2);

            Assert.Equal(2, book.Id);
        }

        [Fact]
        public async Task CheckIfGetByIdReturnsExceptionIfBookNotExist()
        {
            await this.SeedBooks();

            var exception =
               await Assert
                .ThrowsAsync<NullReferenceException>(() => this.bookService.GetByIdAsync<BookViewModel>(2));
            Assert.Equal(string.Format(ExceptionMessages.BookNotFound, 2), exception.Message);
        }
        [Fact]
        public async Task CheckIfGetByAuthorIdReturnsRightBook()
        {
            await this.SeedDatabase();
            await this.SeedBooks();
            await this.SeedBookGenres();
            await this.SeedBookAuthors();

            var author = new Author
            {
                Name = "Adam Brook"
            };
            await authorRepository.AddAsync(author);
            await authorRepository.SaveChangesAsync();


            var third = new Book
            {
                Title = "Right book",
                DateOfPublish = DateTime.UtcNow,
                Description = "Test description second book",
                CoverUrl = TestCoverImageUrl,
            };
            await bookRepository.AddAsync(third);
            await this.bookRepository.SaveChangesAsync();

            var authorBook = new AuthorBook
            {
                AuthorId = author.Id,
                BookId = third.Id,
            };
            await authorBookRepository.AddAsync(authorBook);
            await authorBookRepository.SaveChangesAsync();

            var book = await this.bookService.GetByAuthorIdAsync<BookViewModel>(author.Id);
            var count = book.Count();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfSearchReturnsRightBooks()
        {
            await this.SeedDatabase();
            await this.SeedBooks();
            await this.SeedBookGenres();
            await this.SeedBookAuthors();

            var author = new Author
            {
                Name = "Adam Brook"
            };
            await authorRepository.AddAsync(author);
            await authorRepository.SaveChangesAsync();


            var third = new Book
            {
                Title = "Right book",
                DateOfPublish = DateTime.UtcNow,
                Description = "Test description second book",
                CoverUrl = TestCoverImageUrl,
            };
            await bookRepository.AddAsync(third);
            await this.bookRepository.SaveChangesAsync();

            var authorBook = new AuthorBook
            {
                AuthorId = author.Id,
                BookId = third.Id,
            };
            await authorBookRepository.AddAsync(authorBook);
            await authorBookRepository.SaveChangesAsync();

            var bookByAuthorName = await this.bookService.Search<BookViewModel>(author.Name, 1, 12);
            var count = bookByAuthorName.Count();
            Assert.Equal(1, count);
            var bookByTitle = await this.bookService.Search<BookViewModel>(third.Title, 1, 12);
            var countSecondSearch = bookByTitle.Count();
            Assert.Equal(1, countSecondSearch);
            var bookByRandomChar = await this.bookService.Search<BookViewModel>("Right", 1, 12);
            var counthirdSearch = bookByRandomChar.Count();
            Assert.Equal(1, counthirdSearch);
        }

        [Fact]
        public async Task GetCountShouldReturnCountOfBookss()
        {
            await this.SeedBooks();

            var count = await this.bookService.GetCountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task GetRandomShouldReturnSertainCountOfBooks()
        {
            await this.SeedBooks();
            var third = new Book
            {
                Title = "Right book",
                DateOfPublish = DateTime.UtcNow,
                Description = "Test description second book",
                CoverUrl = TestCoverImageUrl,
            };
            await bookRepository.AddAsync(third);
            await this.bookRepository.SaveChangesAsync();
            var book = await this.bookService.GetByTitle<BookViewModel>("Right book");

            Assert.Equal(1, book.Count());
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

            this.secondBook = new Book
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
            if (this.authorRepository.All().Count() == 0)
            {
                await this.authorRepository.AddAsync(this.firstAuthor);

                await this.authorRepository.SaveChangesAsync();
            }

        }

        private async Task SeedBooks()
        {
            if (this.bookRepository.All().Count() == 0)
            {
                await this.bookRepository.AddAsync(this.secondBook);

                await this.bookRepository.SaveChangesAsync();
            }
        }

        private async Task SeedBookGenres()
        {
            if (this.genreBookRepository.All().Count() == 0)
            {
                await this.genreBookRepository.AddAsync(this.firstBookGenre);

                await this.genreBookRepository.SaveChangesAsync();
            }
        }

        private async Task SeedBookAuthors()
        {
            if (this.authorBookRepository.All().Count() == 0)
            {
                await this.authorBookRepository.AddAsync(this.firstAuthorBook);

                await this.authorBookRepository.SaveChangesAsync();
            }
        }

        private async Task SeedGenres()
        {
            if (this.genreRepository.All().Count() == 0)
            {
                await this.genreRepository.AddAsync(this.firstGenre);

                await this.genreRepository.SaveChangesAsync();
            }
        }


        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("BooksRealm"));
    }
}

