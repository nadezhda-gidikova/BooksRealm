namespace BooksRealm.Data.Common
{
    public static class ExceptionMessages
    {
        public const string MovieNotFound = "Book with id {0} is not found.";

        public const string MovieAlreadyExists = "Book with name {0} already exists";

        public const string DirectorNotFound = "Author with id {0} is not found.";

        public const string InvalidCinemaCategoryType = "Book category type {0} is invalid.";

        public const string GenreNotFound = "Genre with id {0} is not found.";

        public const string GenreAlreadyExists = "Genre with name {0} already exists";

        public const string MovieGenreNotFound = "Book's genre with movie id {0} and genre id {1} is not found.";

        public const string DirectorAlreadyExists = "Author with firstname {0} and lastname {1} already exists";

        public const string CountryAlreadyExists = "Country with name {0} already exists";

        public const string CountryNotFound = "Country with id {0} is not found.";

        public const string CinemaNotFound = "Book with id {0} is not found.";

        public const string FaqNotFound = "Faq with id {0} is not found.";

        public const string FaqAlreadyExists = "Faq with question {0} and answer {1} already exists";

        public const string AlreadySentVote = "You cannot vote twice in the same day. To vote come back again tomorrow at";

        public const string UserShouldBeLoggedIn = "You have to log into your account in order to vote for a movie.";

        public const string AuthenticatedErrorMessage = "Please, login in order to vote.";

        public const string NewsAlreadyExists = "News with title {0} and description {1} already exists";

        public const string NewsNotFound = "News with id {0} is not found.";

 
        public const string PrivacyAlreadyExists = "Privacy with page content {0} already exists";

        public const string PrivacyNotFound = "Privacy with id {0} is not found.";

        public const string PrivacyViewModelNotFound = "Privacy view model is not found.";

        public const string MovieCommentAlreadyExists = "Book comment with book id {0} and content {1} already exists";

        public const string NewsCommentAlreadyExists = "Book comment with news id {0} and content {1} already exists";
    }
}
