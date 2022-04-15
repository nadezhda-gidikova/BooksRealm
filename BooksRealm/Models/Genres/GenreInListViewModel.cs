﻿namespace BooksRealm.Models.Genres
{
    using BooksRealm.Data.Models;
    using BooksRealm.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GenreInListViewModel:IMapFrom<BookGenre>
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
    }
}
