using BooksRealm.Data.Models;
using BooksRealm.Services.Mapping;

namespace BooksRealm.Models.Authors
{
    public class AuthorViewModel : IMapFrom<Author>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
