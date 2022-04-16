namespace BooksRealm.Models.Home
{
    using BooksRealm.Data.Models;
    using BooksRealm.Services.Mapping;
    using System.Collections.Generic;

    public class IndexViewModel:IMapFrom<Book>
    {

        public int BooksCount { get; set; }

        public int ReviewsCount { get; set; }

        public int AuthorsCount { get; set; }

        public int GenresCount { get; set; }

        public IEnumerable<IndexPageBooksViewModel> RandomBooks { get; set; } = new List<IndexPageBooksViewModel>();
    }
}

