namespace BooksRealmTests
{
    using BooksRealm.Data;
    using BooksRealm.Data.Common;
    using BooksRealm.Data.Models;
    using BooksRealm.Data.Repositories;
    using BooksRealm.Models.Authors;
    using BooksRealm.Services;
    using BooksRealm.Services.Mapping;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Xunit;

    public class AuthortorsServiceTests : IDisposable, IClassFixture<Configuration>
    {
        private readonly IAuthorService authorsService;
        private EfDeletableEntityRepository<Author> authorsRepository;
        private SqliteConnection connection;

        private Author firstAuthor;

        public AuthortorsServiceTests()
        {
            InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.authorsService = new AuthorService(this.authorsRepository);
        }

        [Fact]
        public async Task TestAddingDirector()
        {
            var model = new AuthorInputModel
            {
                Name="Angel Minchev"
            };

            await this.authorsService.AddAsync(model.Name);
            var count = await this.authorsRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingOfDirectorProperties()
        {
            var model = new AuthorInputModel
            {
                Name="Sofroni Vrachanski"
            };

            await this.authorsService.AddAsync(model.Name);

            var author = await this.authorsRepository.All().FirstOrDefaultAsync();

            Assert.Equal("Sofroni Vrachanski", author.Name);
            
        }

        [Fact]
        public async Task CheckIfAddingAuthorThrowsArgumentException()
        {
            this.SeedDatabase();

            var author = new AuthorInputModel
            {
                Name = "Sofroni Vrachanski"
            };
            var authorId = await this.authorsService.AddAsync(author.Name);
            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.authorsService.AddAsync(author.Name));
            Assert.Equal(string.Format(ExceptionMessages.AuthorAlreadyExists, author.Name), exception.Message);
        }

        [Fact]
        public async Task CheckIfAddingDirectorReturnsViewModel()
        {
            var author = new AuthorInputModel
            {
                Name = "Sofroni Vrachanski"
            };

            var viewModel = await this.authorsService.AddAsync(author.Name);
            var dbEntry = await this.authorsRepository.All().FirstOrDefaultAsync();

            Assert.Equal(dbEntry.Id, viewModel);
            Assert.Equal(dbEntry.Name, author.Name);
            
        }

        [Fact]
        public async Task CheckIfDeletingDirectorWorksCorrectly()
        {
            this.SeedDatabase();

            await this.authorsService.DeleteAsync(this.firstAuthor.Id);

            var count = await this.authorsRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfDeletingDirectorReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.authorsService.DeleteAsync(3));
            Assert.Equal(string.Format(ExceptionMessages.AuthorNotFound, 3), exception.Message);
        }

       
        [Fact]
        public async Task CheckIfGetAllDirectorsAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var result = await this.authorsService.GetAllAsync<AuthorViewModel>();

            var count = result.Count;
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetAllInListAuthorsAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var result = await this.authorsService.GetAllInLIstAsync<AuthorViewModel>(1,12);

            var count = result.Count();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfCounAuthorsAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var result = await this.authorsService.GetCountAsync();

           
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task CheckIfGetAuthorViewModelByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var expectedModel = new AuthorViewModel
            {
                Id = this.firstAuthor.Id,
                Name = this.firstAuthor.Name,
               
            };

            var viewModel = await this.authorsService.GetByIdAsync<AuthorViewModel>(this.firstAuthor.Id);

            var expectedObj = JsonConvert.SerializeObject(expectedModel);
            var actualResultObj = JsonConvert.SerializeObject(viewModel);

            Assert.Equal(expectedObj, actualResultObj);
        }

        [Fact]
        public async Task CheckIfGetViewModelByIdAsyncThrowsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () =>
                    await this.authorsService.GetByIdAsync<AuthorViewModel>(3));
            Assert.Equal(string.Format(ExceptionMessages.AuthorNotFound, 3), exception.Message);
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

            this.authorsRepository = new EfDeletableEntityRepository<Author>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstAuthor = new Author
            {
                Name="Tosho Toshev"
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedDirectors();
        }

        private async Task SeedDirectors()
        {
            await this.authorsRepository.AddAsync(this.firstAuthor);

            await this.authorsRepository.SaveChangesAsync();
        }

        private static void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("BooksRealm"));
    }
}
