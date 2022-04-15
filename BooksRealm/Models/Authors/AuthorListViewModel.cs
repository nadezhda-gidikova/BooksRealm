namespace BooksRealm.Models.Authors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AuthorListViewModel:PagingViewModel
    {
        public IEnumerable<AuthorViewModel> Authors { get; set; } = new List<AuthorViewModel>();
    }
}
