namespace BooksRealm.Services
{
    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;
    using BooksRealm.Models.Authors;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAuthorService
    {
        public ICollection<AuthorViewModel> GetAllAuthors();
        
    }
}
