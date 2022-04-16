namespace BooksRealmTests
{
    using System;
    using Xunit;

    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using BooksRealm.Services;
    using BooksRealm.Data.Repositories;
    using BooksRealm.Data.Models;
    using System.Reflection;
    using BooksRealm.Services.Mapping;
    using System.Threading.Tasks;
    using BooksRealm.Models.Reviews;
    using BooksRealm.Data;
    using BooksRealm.Data.Common;

    public class ReviewServiceTests : IDisposable
    {
        private const string TestCoverPath = "https://somecoverurl.com";
        private const string TestTrailerPath = "https://sometrailerurl.com";
        private const string TestWallpaperPath = "https://somewallpaperurl.com";

        private readonly IReviewService reviewService;
        private EfDeletableEntityRepository<Review> reviewRepository;
        private EfDeletableEntityRepository<Book> bookRepository;
        private EfDeletableEntityRepository<BooksRealmUser> usersRepository;
        private EfDeletableEntityRepository<Author> authorRepository;
        private SqliteConnection connection;

        private Book firstBook;
        private Author firstAuthor;
        private Review firstReview;
        private BooksRealmUser user;

        public ReviewServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.reviewService = new ReviewService(this.reviewRepository);
        }

        [Fact]
        public async Task TestAddingReview()
        {
            await this.SeedUsers();
            await this.SeedDirectors();
            await this.SeedMovies();

            var review = new ReviewInputModel
            {
                BookId = this.firstBook.Id,
                Content = "I like this movie.",
            };

            await this.reviewService.AddReview(review.Content, this.user.Id, review.BookId);
            var count = await this.reviewRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingOfMovieCommentProperties()
        {
            await this.SeedUsers();
            await this.SeedDirectors();
            await this.SeedMovies();

            var model = new ReviewInputModel
            {
                BookId = this.firstBook.Id,
                Content = "My opinion for the book...",
            };

            await this.reviewService.AddReview( model.Content, this.user.Id, model.BookId);

            var review = await this.reviewRepository.All().FirstOrDefaultAsync();

            Assert.Equal(model.BookId, review.BookId);
            Assert.Equal("My opinion for the book...", review.Content);
        }

        [Fact]
        public async Task CheckIfAddingMovieCommentThrowsArgumentException()
        {
            this.SeedDatabase();

            var review = new ReviewInputModel
            {
                BookId = this.firstBook.Id,
                Content = this.firstReview.Content,
            };

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async ()
                    => await this.reviewService
                    .AddReview(review.Content, this.user.Id, review.BookId));

            Assert.Equal(
                string.Format(
                    ExceptionMessages.ReviewAlreadyExists, review.BookId, review.Content), exception.Message);
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

            this.usersRepository = new EfDeletableEntityRepository<BooksRealmUser>(dbContext);
            this.authorRepository = new EfDeletableEntityRepository<Author>(dbContext);
            this.bookRepository = new EfDeletableEntityRepository<Book>(dbContext);
            this.reviewRepository = new EfDeletableEntityRepository<Review>(dbContext);
        }

        private void InitializeFields()
        {
            this.user = new BooksRealmUser
            {
                Id = "1",
                UserName = "test_email@gmail.com",
                Email = "test_email@gmail.com",
                PasswordHash = "123456",
            };

            this.firstAuthor = new Author
            {
                Id = 1,
                Name="Lesly Braims"
            };

            this.firstBook = new Book
            {
                Id = 1,
                Title = "Avatar",
                DateOfPublish = DateTime.UtcNow,
                Description = "Avatar movie description",
                CoverUrl = TestCoverPath,
            };

            this.firstReview = new Review
            {
                BookId
                = this.firstBook.Id,
                Content = "Test comment here",
                UserId = this.user.Id,
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedUsers();
            await this.SeedDirectors();
            await this.SeedMovies();
            await this.SeedMovieComments();
        }

        private async Task SeedUsers()
        {
            await this.usersRepository.AddAsync(this.user);

            await this.usersRepository.SaveChangesAsync();
        }

        private async Task SeedDirectors()
        {
            await this.authorRepository.AddAsync(this.firstAuthor);

            await this.authorRepository.SaveChangesAsync();
        }

        private async Task SeedMovies()
        {
            await this.bookRepository.AddAsync(this.firstBook);

            await this.bookRepository.SaveChangesAsync();
        }

        private async Task SeedMovieComments()
        {
            await this.reviewRepository.AddAsync(this.firstReview);

            await this.reviewRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("BooksRealm"));

        
    }
}
