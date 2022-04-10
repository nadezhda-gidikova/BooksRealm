using BooksRealm.Data.Common.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksRealm.Data.Models
{
    public class Review: BaseDeletableModel<int>
    {
        public string Content { get; set; }
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual BooksRealmUser User { get; set; }
    }
}