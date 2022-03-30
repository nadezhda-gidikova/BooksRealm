using System;

namespace BooksRealm.Models
{
    public class PagingViewModel
    {
        public int PageNumber { get; set; }

        public bool HasPreviousPage => this.PageNumber > 1;

        public int PreviousPageNumber => this.PageNumber - 1;

        public bool HasNextPage => this.PageNumber < this.PagesCount;

        public int NextPageNumber => this.PageNumber + 1;

        public int PagesCount => (int)Math.Ceiling((double)this.BooksCount / this.ItemsPerPage);

        public int BooksCount { get; set; }

        public int ItemsPerPage { get; set; }
    }
}
