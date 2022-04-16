namespace BooksRealm.Models.Authors
{
    using BooksRealm.Data.Models;
    using BooksRealm.Services.Mapping;

    public class AuthorViewModel : IMapFrom<Author>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
