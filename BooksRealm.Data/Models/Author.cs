using BooksRealm.Data.Common.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BooksRealm.Data.Models
{
    public class Author:BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }
        public virtual ICollection<AuthorBook> Books { get; set; }=new HashSet<AuthorBook>();
    }
}