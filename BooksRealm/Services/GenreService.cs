namespace BooksRealm.Services
{
    using BooksRealm.Data.Common;
    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;
    using BooksRealm.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GenreService:IGenreService
    {
        private readonly IDeletableEntityRepository<Genre> genreRepo;

        public GenreService(IDeletableEntityRepository<Genre> genreRepo)
        {
            this.genreRepo = genreRepo;
        }
        public async Task<ICollection<T>> GetAllAsync<T>()
        {
            return await this.genreRepo.All()
                .OrderBy(x => x.Name)
                .To<T>()                
                .ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllInLIstAsync<T>(int page, int itemsPerPage = 12)
        {
            var authors = await this.genreRepo.All()
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .To<T>()
                .ToListAsync();

            return authors;
        }
        public async Task<T> GetByIdAsync<T>(int id)
        {
            var genre = await this.genreRepo
                 .All()
                 .Where(x => x.Id == id)
                 .To<T>()
                 .FirstOrDefaultAsync();
            if (genre == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.GenreNotFound, id));
            }
            return genre;
        }
        public async Task<int> AddAsync(string name)
        {
            var genre = new Genre
            {
                Name = name,
            };
            bool doesGenreExist = await this.genreRepo
               .All()
               .AnyAsync(x => x.Name==genre.Name);
            if (doesGenreExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.GenreAlreadyExists, genre.Name));
            }
            await this.genreRepo.AddAsync(genre);
           await this.genreRepo.SaveChangesAsync();
            return genre.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var genre = await this.genreRepo
                   .All()
                   .Where(x => x.Id == id)
                   .FirstOrDefaultAsync();
            if (genre == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.GenreNotFound, id));
            }

            this.genreRepo.Delete(genre);
            await this.genreRepo.SaveChangesAsync();
            return genre.Id;
        }
        public async Task<int> GetCountAsync()
        {
            return await this.genreRepo.AllAsNoTracking().CountAsync();
        }
    }
}
