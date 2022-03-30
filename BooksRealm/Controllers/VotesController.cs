namespace BookRealm.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using BooksRealm.Services;
    using BooksRealm.Models.Votes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    

    [ApiController]
    [Route("api/[controller]")]
    
    public class VotesController : ControllerBase
    {
        private readonly IVotesService votesService;

        public VotesController(IVotesService votesService)
        {
            this.votesService = votesService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PostVoteResponseModel>> Post(PostVoteInputModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.votesService.SetVoteAsync(input.BookId, userId, input.Value);
            var averageVotes = this.votesService.GetAverageVotes(input.BookId);
            return new PostVoteResponseModel { AverageVote = averageVotes };
        }
    }
}
