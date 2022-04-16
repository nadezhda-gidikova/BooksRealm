namespace BooksRealm.Models.Genres
{
    using System.ComponentModel.DataAnnotations;
    using static BooksRealm.Data.DataConstants.Genre;

    public class GenreInputModel
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }
    }
}
