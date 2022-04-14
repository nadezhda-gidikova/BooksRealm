namespace BooksRealm.Data
{
    public class DataConstants
    {
        public class User
        {
            public const int FullNameMinLength = 5;
            public const int FullNameMaxLength = 60;
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
        }

        public class Book
        {
            public const int TitleMinLength = 2;
            public const int TitleMaxLength = 60;
            public const int ModelMinLength = 2;
            public const int ModelMaxLength = 30;
            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 10000;
            public const int YearMinValue = 2000;
            public const int YearMaxValue = 2050;
        }

        public class Category
        {
            public const int NameMaxLength = 40;
        }
        public class Genre
        {
            public const int NameMaxLength = 80;
        }
        public class Review
        {
            public const int ContentMinLength = 10;

            public const int ContentMaxLength = 10000;
        }
        public static class ContactFormEntry
        {
            public const int NameMaxLength = 60;
            public const int SubjectMaxLength = 100;
            public const int ContentMaxLength = 10000;
        }

    }
}
