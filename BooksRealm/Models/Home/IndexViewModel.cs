using AutoMapper;
using BooksRealm.Data.Models;
using BooksRealm.Services.Mapping;
using System.Collections.Generic;

namespace BooksRealm.Models.Home
{
    public class IndexViewModel:IMapFrom<Book>//.IHaveCustomMappings
    {
        public IEnumerable<IndexPageBooksViewModel> RandomBooks { get; set; }=new List<IndexPageBooksViewModel>();

        public int BooksCount { get; set; }

        public int ReviewsCount { get; set; }

        public int AuthorsCount { get; set; }
        public int GenresCount { get; set; }

            

    }
}

