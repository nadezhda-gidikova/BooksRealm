namespace BooksRealm.Data.Models
{
    using BooksRealm.Data.Common.Models;
    using static BooksRealm.Data.DataConstants.Genre;

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Genre: BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }
        public virtual ICollection<BookGenre> Books { get; set; }=new HashSet<BookGenre>();
    }
}