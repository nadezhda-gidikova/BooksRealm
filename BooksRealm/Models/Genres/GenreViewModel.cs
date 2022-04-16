namespace BooksRealm.Models.Genres
{
    using BooksRealm.Data.Models;
    using BooksRealm.Services.Mapping;

    public class GenreViewModel:IMapFrom<Genre>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
