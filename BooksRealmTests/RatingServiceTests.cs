using BooksRealm.Data;
using BooksRealm.Data.Common;
using BooksRealm.Data.Models;
using BooksRealm.Data.Repositories;
using BooksRealm.Services;
using BooksRealm.Services.Mapping;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace BooksRealmTests
{
    public class RatingsServiceTests : IDisposable
    {
        private const string TestCoverImageUrl = "https://someurl.com";
        private readonly IRatingsService ratingsService;
        private EfDeletableEntityRepository<Vote> starRatingsRepository;
        private EfDeletableEntityRepository<Book> bookRepository;
        private EfDeletableEntityRepository<Author> authorRepository;
        private EfDeletableEntityRepository<BooksRealmUser> usersRepository;
        private SqliteConnection connection;

        private Vote firstStarRating;
        private Author firstAuthor;
        private Book firstBook;
        private BooksRealmUser user;

        public RatingsServiceTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.ratingsService = new RatingsService(this.starRatingsRepository,this.bookRepository);
        }

        [Fact]
        public async Task CheckIfVoteAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            await this.ratingsService.VoteAsync(this.firstBook.Id, this.user.Id, 5);
            var count = await this.starRatingsRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfVoteAsyncThrowsArgumentException()
        {
            this.SeedDatabase();
            await this.SeedStarRatings();

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.ratingsService.VoteAsync(this.firstBook.Id, this.user.Id, 6));
            Assert.Equal(ExceptionMessages.AlreadySentVote, exception.Message);
        }

        [Fact]
        public async Task CheckIfVoteAsyncWorksCorrectlyAfterSecondVoting()
        {
            this.SeedDatabase();
            var starRating = new Vote
            {
                Value = 5,
                BookId = 1,
                UserId = this.user.Id,
                NextDateRate = DateTime.UtcNow.AddDays(-1),
            };
            await this.starRatingsRepository.AddAsync(starRating);
            await this.starRatingsRepository.SaveChangesAsync();

            await this.ratingsService.VoteAsync(this.firstBook.Id, this.user.Id, 5);
            var count = await this.starRatingsRepository.All().CountAsync();
            var currentStarRating = await this.starRatingsRepository.All().FirstOrDefaultAsync();

            Assert.Equal(1, count);
            Assert.Equal(5, currentStarRating.Value);
            Assert.Equal(starRating.NextDateRate, currentStarRating.NextDateRate);
        }

        [Fact]
        public async Task CheckIfGetStarRatingsAsyncWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedStarRatings();

            var result = await this.ratingsService.GetStarRatingsAsync(this.firstBook.Id);

            Assert.Equal(5, result);
        }

        [Fact]
        public async Task CheckIfGetNextVoteDateAsyncWorksCorrectly()
        {
            this.SeedDatabase();
            await this.SeedStarRatings();

            var result = await this.ratingsService.GetNextVoteDateAsync(this.firstBook.Id, this.user.Id);

            Assert.Equal(this.firstStarRating.NextDateRate, result);
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
            this.starRatingsRepository = new EfDeletableEntityRepository<Vote>(dbContext);
            this.bookRepository = new EfDeletableEntityRepository<Book>(dbContext);
            this.authorRepository = new EfDeletableEntityRepository<Author>(dbContext);
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
                Name = "Simeon Kirilov",
            };

            this.firstBook = new Book
            {
               
                Title = "Right book",
                DateOfPublish = DateTime.UtcNow,
                Description = "Test description second book",
                CoverUrl = TestCoverImageUrl,
            };
            
            this.firstStarRating = new Vote
            {
                Value = 5,
                BookId = 1,
                UserId = this.user.Id,
                NextDateRate = DateTime.UtcNow.AddDays(1),
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedAuthors();
            await this.SeedMovies();
            await this.SeedUsers();
        }

        private async Task SeedUsers()
        {
            await this.usersRepository.AddAsync(this.user);

            await this.usersRepository.SaveChangesAsync();
        }

        private async Task SeedStarRatings()
        {
            await this.starRatingsRepository.AddAsync(this.firstStarRating);

            await this.starRatingsRepository.SaveChangesAsync();
        }

        private async Task SeedMovies()
        {
            await this.bookRepository.AddAsync(this.firstBook);

            await this.bookRepository.SaveChangesAsync();
        }

        private async Task SeedAuthors()
        {
            await this.authorRepository.AddAsync(this.firstAuthor);

            await this.authorRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("BooksRealm"));
    }
}
