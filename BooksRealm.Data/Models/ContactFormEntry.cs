using BooksRealm.Data.Common.Models;
using static BooksRealm.Data.DataConstants.ContactFormEntry;

using System.ComponentModel.DataAnnotations;

namespace BooksRealm.Data.Models
{
    public class ContactFormEntry:BaseModel<int>
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(SubjectMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

    }
}
