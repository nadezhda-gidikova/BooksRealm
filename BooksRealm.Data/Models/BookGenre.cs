namespace BooksRealm.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class BookGenre
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        [ForeignKey(nameof(Genre))]
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
