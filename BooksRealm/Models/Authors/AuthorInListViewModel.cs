namespace BooksRealm.Models.Authors
{
    using BooksRealm.Data.Models;
    using BooksRealm.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AuthorInListViewModel:IMapFrom<AuthorBook>
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
