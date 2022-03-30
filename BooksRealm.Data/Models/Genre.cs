using BooksRealm.Data.Common.Models;
using System.Collections.Generic;

namespace BooksRealm.Data.Models
{
    public class Genre: BaseDeletableModel<int>
    {
        public string Name { get; set; }
        public virtual ICollection<BookGenre> Books { get; set; }=new HashSet<BookGenre>();
    }
}