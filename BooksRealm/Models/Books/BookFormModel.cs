using BooksRealm.Models.Authors;
using BooksRealm.Models.Genres;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BooksRealm.Data;

namespace BooksRealm.Models.Books
{
   

    public class BookFormModel
    {
        [Required]
        [MinLength(DataConstants.Book.TitleMinLength)]
        [MaxLength(DataConstants.Book.TitleMaxLength)]

        public string Title { get; set; }
        [Required]
        [MinLength(DataConstants.Book.DescriptionMinLength)]
      
        public string Description { get; set; }
        [Display(Name ="Published on")]
        [Required]
        public DateTime DateOfPublish { get; set; }
        [Required]
        public string CoverUrl { get; set; }
        [Display(Name ="Author")]
        [Required]
        public int AuthorId { get; set; }   
        [Display(Name ="Category")]
        [Required]
        public int GenreId { get; set; }
        public virtual ICollection<AuthorViewModel> Authors { get; set; } = new HashSet<AuthorViewModel>();
        public virtual ICollection<GenreViewModel> Genres { get; set; } = new HashSet<GenreViewModel>();
    }
}
