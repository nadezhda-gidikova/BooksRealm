namespace BooksRealm.Models.Genres
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static BooksRealm.Data.DataConstants.Genre;

    using System.Linq;
    using System.Threading.Tasks;

    public class GenreInputModel
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }
    }
}
