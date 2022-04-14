namespace BooksRealm.Models.Ratings
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RatingInputModel
    {
        public int BookId { get; set; }
        [Range(1,5)]
        public int Value { get; set; }
    }
}
