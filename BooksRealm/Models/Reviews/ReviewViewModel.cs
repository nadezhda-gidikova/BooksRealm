using BooksRealm.Data.Models;
using BooksRealm.Services.Mapping;
using Ganss.XSS;
using System;

namespace BooksRealm.Models.Reviews
{
    public class ReviewViewModel:IMapFrom<Review>
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserUserName { get; set; }
        public string SanitaizedContent => new HtmlSanitizer().Sanitize(this.Content);
        public DateTime CreatedOn { get; set; }
    }
}
