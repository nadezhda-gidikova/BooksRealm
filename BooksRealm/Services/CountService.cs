using BooksRealm.Data.Common.Repositories;
using BooksRealm.Data.Models;
using BooksRealm.Services.Mapping;
using BooksRealm.Services.Mapping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksRealm.Services
{
    public class CountService : ICountService
    {
        private readonly IDeletableEntityRepository<Book> booksRepository;
        private readonly IDeletableEntityRepository<Author> authorRepo;
        private readonly IDeletableEntityRepository<Review> reviewsRepo;
        private readonly IDeletableEntityRepository<Genre> genresRepo;

        public CountService(IDeletableEntityRepository<Book> booksRepository
            ,IDeletableEntityRepository<Author>authorRepo
            , IDeletableEntityRepository<Review> reviewsRepo
            , IDeletableEntityRepository<Genre> genresRepo)
        {
            this.booksRepository = booksRepository;
            this.authorRepo = authorRepo;
            this.reviewsRepo = reviewsRepo;
            this.genresRepo = genresRepo;
        }
        public CountDto GetCounts()
        {
            var data = new CountDto
            {
                 BooksCount= this.booksRepository.All().Count(),
                AuthorsCount=this.authorRepo.All().Count(),
                GenresCount=this.genresRepo.All().Count(),
                ReviewsCount=this.reviewsRepo.All().Count(),
            };

            return data;
        }
    }
}
