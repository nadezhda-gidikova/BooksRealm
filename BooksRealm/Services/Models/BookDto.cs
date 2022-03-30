using BooksRealm.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class BookDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public DateTime DateOfPublish { get; set; }
        public string CoverUrl { get; set; }
        public List<string> Authors { get; set; } = new List<string>();
        public List<string> Reviews { get; set; } = new List<string>();
        public List<string> Genres { get; set; } = new List<string>();

    }
}
