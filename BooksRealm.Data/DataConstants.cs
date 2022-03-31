﻿namespace BooksRealm.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DataConstants
    {
        public class User
        {
            public const int FullNameMinLength = 5;
            public const int FullNameMaxLength = 40;
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
        }

        public class Book
        {
            public const int TitleMinLength = 2;
            public const int TitleMaxLength = 20;
            public const int ModelMinLength = 2;
            public const int ModelMaxLength = 30;
            public const int DescriptionMinLength = 10;
            public const int YearMinValue = 2000;
            public const int YearMaxValue = 2050;
        }

        public class Category
        {
            public const int NameMaxLength = 25;
        }
        public class Genre
        {
            public const int NameMaxLength = 25;
        }
    }
}
