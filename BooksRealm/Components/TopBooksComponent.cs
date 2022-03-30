using BooksRealm.Models.Books;
using BooksRealm.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksRealm.Components
{
    [ViewComponent(Name ="TopBooksOf2021")]
    public class TopBooksComponent:ViewComponent
    {
        private readonly IBookService bookService;

        public TopBooksComponent(IBookService bookService)
        {
            this.bookService = bookService;
        }
        public IViewComponentResult Invoke()
        {
            var books = bookService.GetByCategory<BookInListViewModel>("Top 20 Best Books of 2021");
            return View(books);
        }
    }
}
