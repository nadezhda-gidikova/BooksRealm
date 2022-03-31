﻿using BooksRealm.Data.Models;
using BooksRealm.Services.Mapping;

namespace BooksRealm.Models.Genres
{
    public class GenreViewModel:IMapFrom<BookGenre>
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
    }
}
