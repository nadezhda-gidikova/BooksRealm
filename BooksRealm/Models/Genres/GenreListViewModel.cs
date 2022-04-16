namespace BooksRealm.Models.Genres
{
    using System.Collections.Generic;

    public class GenreListViewModel:PagingViewModel
    {
        public IEnumerable<GenreViewModel> Genres { get; set; } = new List<GenreViewModel>();
    }
}
