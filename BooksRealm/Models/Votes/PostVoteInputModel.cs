namespace BooksRealm.Models.Votes
{
    using System.ComponentModel.DataAnnotations;

    public class PostVoteInputModel
    {
        public int BookId { get; set; }

        [Range(1, 5)]
        public byte Value { get; set; }
    }
}
