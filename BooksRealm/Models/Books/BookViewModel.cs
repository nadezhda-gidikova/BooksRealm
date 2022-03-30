using AutoMapper;
using BooksRealm.Data.Models;
using BooksRealm.Services.Mapping;
using BooksRealm.Models.Authors;
using BooksRealm.Models.Genres;
using BooksRealm.Models.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BooksRealm.Models.Books
{
    public class BookViewModel:IMapFrom<Book>,IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }   
        public DateTime DateOfPublish { get; set; }
        public string CoverUrl { get; set; }
        public double Vote { get; set; }
        public double AverageVote { get; set; }
        public int StarRatingsSum { get; set; }  

        public ICollection<AuthorViewModel> Authors { get; set; } = new HashSet<AuthorViewModel>();
        public ICollection<ReviewViewModel> Reviews { get; set; } = new HashSet<ReviewViewModel>();
        public ICollection<GenreViewModel> Genres { get; set; } = new HashSet<GenreViewModel>();
        //public void CreateMappings(IProfileExpression configuration)
        //{
        //    configuration.CreateMap<Book, BookViewModel>()
        //        .ForMember(x => x.StarRatingsSum, options =>
        //        {
        //            options.MapFrom(m => m.Votes.Sum(st => st.Value));
        //        });
        //}
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Book, BookViewModel>()
                .ForMember(x => x.AverageVote, opt =>
                    opt.MapFrom(x => x.Votes.Count() == 0 ? 0 : x.Votes.Average(v => v.Value)));

        }



    }
}
