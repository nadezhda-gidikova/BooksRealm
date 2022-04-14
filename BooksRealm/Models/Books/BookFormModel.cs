using BooksRealm.Models.Authors;
using BooksRealm.Models.Genres;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BooksRealm.Data;
using BooksRealm.Services.Mapping;
using static BooksRealm.Data.DataConstants.Book;


using AutoMapper;
using BooksRealm.Data.Models;

namespace BooksRealm.Models.Books
{


    public class BookFormModel : IMapFrom<Book>//,IHaveCustomMappings
    {
        [Required]
        [MinLength(TitleMinLength)]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MinLength(DescriptionMinLength)]
        [MaxLength(DescriptionMaxLength)]
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
