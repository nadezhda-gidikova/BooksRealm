namespace BooksRealm.Data.Models
{
    using BooksRealm.Data.Common.Models;
    using static BooksRealm.Data.DataConstants.ContactFormEntry;

    using System.ComponentModel.DataAnnotations;

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
        public string Subject { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

    }
}
