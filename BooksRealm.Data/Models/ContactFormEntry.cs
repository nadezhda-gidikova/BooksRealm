using BooksRealm.Data.Common.Models;

namespace BooksRealm.Data.Models
{
    public class ContactFormEntry:BaseModel<int>
    {
        public string Name { get; set; }

        public string  Email { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Ip { get; set; }

    }
}
