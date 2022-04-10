namespace BooksRealm.Models.Reviews
{
    using Ganss.XSS;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
   
    public class ReviewInputModel
    {
        public string Content { get; set; }
        
        public int BookId { get; set; }

        public string UserId { get; set; }
    }
}
