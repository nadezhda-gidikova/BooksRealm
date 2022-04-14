namespace BooksRealm.Models.Authors
{
    using System.ComponentModel.DataAnnotations;
    using static BooksRealm.Data.DataConstants.User;

    public class AuthorInputModel
    {
        [Required]
        [MinLength(FullNameMinLength)]
        [MaxLength(FullNameMaxLength)]
        public string Name { get; set; }
    }
}
