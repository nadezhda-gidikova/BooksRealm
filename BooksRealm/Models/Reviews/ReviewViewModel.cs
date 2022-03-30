using BooksRealm.Data.Models;
using BooksRealm.Services.Mapping;

namespace BooksRealm.Models.Reviews
{
    public class ReviewViewModel:IMapFrom<Review>
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
    }
}
