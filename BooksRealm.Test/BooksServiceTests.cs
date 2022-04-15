namespace BooksRealm.Test
{
    using BooksRealm.Data;
    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;
    using BooksRealm.Data.Repositories;
    using BooksRealm.Models.Books;
    using BooksRealm.Services;
    using BooksRealm.Services.Mapping;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    class BooksServiceTests
    {
        private const string TestCoverImageUrl = "https://someurl.com";
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        private Book secondBook;
        private Genre firstGenre;
        private Author firstAuthor;
        private BookGenre firstBookGenre;
        private AuthorBook firstAuthorBook;
        public BooksServiceTests()
        {

        }
        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                 .AddSingleton(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>))
                .AddSingleton(typeof(IRepository<>), typeof(EfRepository<>))
                 .AddSingleton<IBookService, BookService>()
                .BuildServiceProvider();

            var bookRepo = serviceProvider.GetService<EfDeletableEntityRepository<Book>>();
            var authorRepository = serviceProvider.GetService<EfDeletableEntityRepository<Author>>();
            var authorBookRepository = serviceProvider.GetService<EfRepository<AuthorBook>>();
            var genreRepository = serviceProvider.GetService<EfDeletableEntityRepository<Genre>>();
            var bookGenreRepo = serviceProvider.GetService<EfRepository<BookGenre>>();
            var bookService = new BookService(bookRepo, authorRepository, genreRepository
                , bookGenreRepo, authorBookRepository);
            InitializeFields();
        }
        [Test, RequiresThread]
        public async Task TestAddingBook()
        {
            //await SeedDatabase(authorRepository,genreRepository);

            var model = new BookFormModel
            {
                Title = "Spy",
                DateOfPublish = DateTime.UtcNow,
                CoverUrl = TestCoverImageUrl,
                Description = "Test description here",
                GenreId = 1,
                AuthorId = 1,
            };

            //var book = await bookService.CreateAsync(model);

            //var count = await this.bookRepository.All().CountAsync();

            //Assert.AreEqual(1, count);
        }
        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
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

        private async Task SeedDatabase(EfDeletableEntityRepository<Author> authorRepository, EfRepository<Genre> genreRepository)
        {
            await SeedAuthors(authorRepository);
            await SeedGenres(genreRepository);
        }

        private async Task SeedAuthors(EfDeletableEntityRepository<Author> authorRepository)
        {
            await authorRepository.AddAsync(this.firstAuthor);

            await authorRepository.SaveChangesAsync();
        }

        private async Task SeedBooks(EfDeletableEntityRepository<Book> bookRepository)
        {
            await bookRepository.AddAsync(this.secondBook);

            await bookRepository.SaveChangesAsync();
        }

        private async Task SeedBookGenres(EfRepository<BookGenre> genreBookRepository)
        {
            await genreBookRepository.AddAsync(this.firstBookGenre);

            await genreBookRepository.SaveChangesAsync();
        }

        private async Task SeedBookAuthors(EfRepository<AuthorBook> authorBookRepository)
        {
            await authorBookRepository.AddAsync(this.firstAuthorBook);

            await authorBookRepository.SaveChangesAsync();
        }

        private async Task SeedGenres(EfRepository<Genre> genreRepository)
        {
            await genreRepository.AddAsync(this.firstGenre);

            await genreRepository.SaveChangesAsync();
        }


        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("BooksRealm"));
    }
}

