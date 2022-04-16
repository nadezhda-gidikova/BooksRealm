namespace BooksRealm.Data.Models
{
    using BooksRealm.Data.Common.Models;
    using static BooksRealm.Data.DataConstants.Review;

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Review: BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        [ForeignKey(nameof(User))]
        
        public string UserId { get; set; }
        public virtual BooksRealmUser User { get; set; }
    }
}