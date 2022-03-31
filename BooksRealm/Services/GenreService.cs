namespace BooksRealm.Services
{
    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;
    using BooksRealm.Models.Genres;
    using BooksRealm.Services.Mapping;
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
        public ICollection<GenreViewModel> GetAll()
        {
            return this.genreRepo.AllAsNoTracking()
                .Select(x => new GenreViewModel
                {
                   GenreId= x.Id,
                    GenreName=x.Name,
                })
                .OrderBy(x => x.GenreName)
                .ToList();
        }
        public T GetById<T>(int id)
        {
            var author = this.genreRepo
                 .AllAsNoTracking()
                 .Where(x => x.Id == id)
                 .To<T>()
                 .FirstOrDefault();

            return author;
        }
        public int Add(string name)
        {
            var genre = new Genre
            {
                Name = name,
            };
            this.genreRepo.AddAsync(genre);
            this.genreRepo.SaveChangesAsync();
            return genre.Id;
        }

        public int Delete(int id)
        {
            var author = this.genreRepo
                   .AllAsNoTracking()
                   .Where(x => x.Id == id)
                   .FirstOrDefault();
            this.genreRepo.Delete(author);
            return author.Id;
        }
    }
}
