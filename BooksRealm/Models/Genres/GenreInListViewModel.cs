namespace BooksRealm.Models.Genres
{
    using BooksRealm.Data.Models;
    using BooksRealm.Services.Mapping;

    public class GenreInListViewModel:IMapFrom<BookGenre>
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
    }
}
