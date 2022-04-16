namespace BooksRealmTests
{
    using BooksRealm.Data;
    using BooksRealm.Data.Common;
    using BooksRealm.Data.Models;
    using BooksRealm.Data.Repositories;
    using BooksRealm.Models.Authors;
    using BooksRealm.Models.Genres;
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

    public class GenreServiceTests : IDisposable, IClassFixture<Configuration>
    {
        private readonly IGenreService genresService;
        private EfDeletableEntityRepository<Genre> genreRepository;
        private SqliteConnection connection;

        private Genre firstGenre;

        public GenreServiceTests()
        {
            InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();

            this.genresService = new GenreService(this.genreRepository);
        }

        [Fact]
        public async Task TestAddingGenre()
        {
            var model = new GenreInputModel
            {
                Name = "Romance"
            };

            await this.genresService.AddAsync(model.Name);
            var count = await this.genreRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckSettingOfGenreProperties()
        {
            var model = new GenreInputModel
            {
                Name = "Romance"
            };

            await this.genresService.AddAsync(model.Name);

            var genre = await this.genreRepository.All().FirstOrDefaultAsync();

            Assert.Equal("Romance", genre.Name);

        }

        [Fact]
        public async Task CheckIfAddingGenreThrowsArgumentException()
        {
            this.SeedDatabase();

            var genre = new GenreInputModel
            {
                Name = "Romance"
            };
            var authorId = await this.genresService.AddAsync(genre.Name);
            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await this.genresService.AddAsync(genre.Name));
            Assert.Equal(string.Format(ExceptionMessages.GenreAlreadyExists, genre.Name), exception.Message);
        }

        [Fact]
        public async Task CheckIfAddingGenrerReturnsViewModel()
        {
            var genre = new AuthorInputModel
            {
                Name = "Romance"
            };

            var viewModel = await this.genresService.AddAsync(genre.Name);
            var dbEntry = await this.genreRepository.All().FirstOrDefaultAsync();

            Assert.Equal(dbEntry.Id, viewModel);
            Assert.Equal(dbEntry.Name, genre.Name);

        }

        [Fact]
        public async Task CheckIfDeletingGenreWorksCorrectly()
        {
            this.SeedDatabase();

            await this.genresService.DeleteAsync(this.firstGenre.Id);

            var count = await this.genreRepository.All().CountAsync();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task CheckIfDeletingGenreReturnsNullReferenceException()
        {
            this.SeedDatabase();

            var exception = await Assert
                .ThrowsAsync<NullReferenceException>(async () => await this.genresService.DeleteAsync(3));
            Assert.Equal(string.Format(ExceptionMessages.GenreNotFound, 3), exception.Message);
        }


        [Fact]
        public async Task CheckIfGetAllGenresAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var result = await this.genresService.GetAllAsync<GenreViewModel>();

            var count = result.Count;
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CheckIfGetGenreViewModelByIdAsyncWorksCorrectly()
        {
            this.SeedDatabase();

            var expectedModel = new GenreViewModel
            {
                Id = this.firstGenre.Id,
                Name = this.firstGenre.Name,

            };

            var viewModel = await this.genresService.GetByIdAsync<GenreViewModel>(this.firstGenre.Id);

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
                    await this.genresService.GetByIdAsync<GenreViewModel>(3));
            Assert.Equal(string.Format(ExceptionMessages.GenreNotFound, 3), exception.Message);
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

            this.genreRepository = new EfDeletableEntityRepository<Genre>(dbContext);
        }

        private void InitializeFields()
        {
            this.firstGenre = new Genre
            {
                Name = "Horror"
            };
        }

        private async void SeedDatabase()
        {
            await this.SeedGenres();
        }

        private async Task SeedGenres()
        {
            await this.genreRepository.AddAsync(this.firstGenre);

            await this.genreRepository.SaveChangesAsync();
        }

        private static void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("BooksRealm"));
    }
}

