using BooksRealm.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BooksRealm.Data.Models
{
    public class Book:BaseDeletableModel<int>
    {
        
        
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public double Rating { get; set; }
        public DateTime DateOfPublish { get; set; }
        public string CoverUrl { get; set; }
        public virtual ICollection<AuthorBook> Authors { get; set; } = new HashSet<AuthorBook>();
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<BookGenre> Genres { get; set; } = new HashSet<BookGenre>();

       public virtual ICollection<Vote> Votes { get; set; } = new HashSet<Vote>();


    }
}
