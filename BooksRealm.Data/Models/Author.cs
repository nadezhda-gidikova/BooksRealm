using BooksRealm.Data.Common.Models;
using static BooksRealm.Data.DataConstants.User;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BooksRealm.Data.Models
{
    public class Author:BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(FullNameMaxLength)]
        public string Name { get; set; }
        public virtual ICollection<AuthorBook> Books { get; set; } =new HashSet<AuthorBook>();
    }
}