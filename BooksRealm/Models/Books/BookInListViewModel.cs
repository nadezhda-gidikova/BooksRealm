using AutoMapper;
using BooksRealm.Data.Models;
using BooksRealm.Models.Authors;
using BooksRealm.Services.Mapping;
using System;
using System.Collections.Generic;

namespace BooksRealm.Models.Books
{
    public class BookInListViewModel : IMapFrom<Book>//, IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public DateTime DateOfPublish { get; set; }
        public string CoverUrl { get; set; }
        public string Search { get; set; }
        public string CategoryName { get; set; }    
        public IEnumerable<AuthorViewModel> Authors { get; set; } = new HashSet<AuthorViewModel>();

        
    }
}
