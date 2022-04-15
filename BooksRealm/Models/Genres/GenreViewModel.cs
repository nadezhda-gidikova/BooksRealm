using BooksRealm.Data.Models;
using BooksRealm.Services.Mapping;

namespace BooksRealm.Models.Genres
{
    public class GenreViewModel:IMapFrom<Genre>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
