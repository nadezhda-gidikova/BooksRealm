using BooksRealm.Data.Common.Models;

namespace BooksRealm.Data.Models
{
    public class Review: BaseDeletableModel<int>
    {
        public string Text { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        public string UserId { get; set; }
        public BooksRealmUser User { get; set; }
    }
}