namespace BooksRealm.Models.Reviews
{
    using System.ComponentModel.DataAnnotations;
    using static BooksRealm.Data.DataConstants.Review;

    public class ReviewInputModel
    {
        [Required]
        [MinLength(ContentMinLength)]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }
       
        public int BookId { get; set; }

        public string UserId { get; set; }
    }
}
