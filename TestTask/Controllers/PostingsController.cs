using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TestTask.PostingsClient.Contracts;
using TestTask.Validation;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostingsController : ControllerBase
    {
        private readonly IValidator<PageRequest> _pageRequestValidator;
        private IPostingsService _postingsService;
        public PostingsController(IValidator<PageRequest> pageRequestValidator, IPostingsService postingsService)
        {
            _pageRequestValidator = pageRequestValidator;
            _postingsService = postingsService;
        }

        [HttpGet]
        public async Task<PageResponse<Posting>> GetPostings([FromQuery]PageRequest pageRequest)
        {
            await _pageRequestValidator.ValidateAndThrowAsync(pageRequest);
            var postings = await _postingsService.GetPostings(pageRequest);
            return postings;
        }
    }
}
