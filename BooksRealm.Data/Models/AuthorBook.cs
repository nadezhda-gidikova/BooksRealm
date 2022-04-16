﻿namespace BooksRealm.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class AuthorBook
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }
        [ForeignKey(nameof(Book))]
        public int  BookId { get; set; }
        public virtual Book Book { get; set; }
            
    }
}
