namespace BooksRealm.Models.Books
{
    using System.Collections.Generic;

    public class BookListViewModel: PagingViewModel
    {  
            public IEnumerable<BookInListViewModel> Books { get; set; }=new List<BookInListViewModel>();
        
    }
}
