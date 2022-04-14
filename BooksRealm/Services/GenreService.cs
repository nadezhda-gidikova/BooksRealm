namespace BooksRealm.Services
{
    using BooksRealm.Data.Common;
    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;
    using BooksRealm.Models.Genres;
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
            return genre.Id;
        }
    }
}
