using BooksRealm.Data.Models;
using BooksRealm.Services.Mapping;

namespace BooksRealm.Models.Authors
{
    public class AuthorViewModel : IMapFrom<AuthorBook>
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }

}
