namespace BooksRealm.Models.Ratings
{
    using System;

    public class StarRatingResponseModel
    {
        public double StarRatingsSum { get; set; }

        public string ErrorMessage { get; set; }

        public string AuthenticateErrorMessage { get; set; }

        public DateTime NextVoteDate { get; set; }
    }
}
