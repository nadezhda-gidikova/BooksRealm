namespace BooksRealm.Data.Models
{
    using BooksRealm.Data.Common.Models;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Vote: BaseDeletableModel<int>
    {
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual BooksRealmUser User { get; set; }
        public int Value { get; set; }
        public DateTime NextDateRate { get; set; }
    }
}
