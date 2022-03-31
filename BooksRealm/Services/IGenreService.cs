namespace BooksRealm.Services
{
    using BooksRealm.Models.Genres;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IGenreService
   {
       public ICollection<GenreViewModel> GetAllAsKeyValuePairs();
        
    }
}
