namespace BooksRealm.Models.Reviews
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReviewInputModel
    {
        public string ReviewsContent { get; set; }    
        public int BookId { get; set; }
        public string UserId { get; set; }
    }
}
