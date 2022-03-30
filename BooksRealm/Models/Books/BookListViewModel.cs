using System.Collections.Generic;

namespace BooksRealm.Models.Books
{
    public class BookListViewModel: PagingViewModel
    {  
            public IEnumerable<BookInListViewModel> Books { get; set; }=new List<BookInListViewModel>();
        
    }
}
