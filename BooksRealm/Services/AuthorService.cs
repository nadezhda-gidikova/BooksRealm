namespace BooksRealm.Services
{
    using BooksRealm.Data.Common.Repositories;
    using BooksRealm.Data.Models;
    using BooksRealm.Models.Authors;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AuthorService:IAuthorService
    {
        private readonly IDeletableEntityRepository<Author> authorRepo;

        public AuthorService(IDeletableEntityRepository<Author> authorRepo)
        {
            this.authorRepo = authorRepo;
        }
        public ICollection<AuthorViewModel> GetAllAuthors()    
        {
            return this.authorRepo.AllAsNoTracking()
                .Select(x => new AuthorViewModel
                {
                    AuthorId=x.Id,
                   AuthorName= x.Name,
                })
                .OrderBy(x => x.AuthorName)
                .ToList();
        }
    }
}
