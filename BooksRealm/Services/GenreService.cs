namespace BooksRealm.Services
{
    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;
    using BooksRealm.Models.Genres;
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
        public ICollection<GenreViewModel> GetAllAsKeyValuePairs()
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
    }
}
