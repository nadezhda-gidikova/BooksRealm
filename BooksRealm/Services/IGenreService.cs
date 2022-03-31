namespace BooksRealm.Services
{
    using BooksRealm.Models.Genres;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IGenreService
   {
       public ICollection<GenreViewModel> GetAll();
        public T GetById<T>(int id);
        public int Add(string name);
        public int Delete(int id);

    }
}
