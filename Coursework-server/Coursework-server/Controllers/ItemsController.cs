using Coursework_server.Commands;
using Coursework_server.Data.ViewModels;
using Coursework_server.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coursework_server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ItemsController : ControllerBase
	{
        private readonly IMediator _mediator;
        
		public ItemsController(IMediator mediator)
		{
            _mediator = mediator;
		}

        [HttpPost("create")]
        [Authorize]
        public void CreateItem([FromBody] CreateItemCommand request)
        {
            request.CurrentUserId = HttpContext.User.Identity?.Name;
            
            _mediator.Send(request, HttpContext.RequestAborted);
        }

        [HttpGet("short")]
		public Task<List<ShortItemVm>> GetShortItems([FromQuery] Guid collectionId) =>
			_mediator.Send(new GetShortItemsQuery(collectionId), HttpContext.RequestAborted);

        [HttpGet("search")]
        public Task<List<ShortItemVm>> SearchItems([FromQuery] SearchItemsQuery request) =>
            _mediator.Send(request, HttpContext.RequestAborted);

        [HttpGet("last")]
        public Task<List<ShortItemVm>> GetLastAddedItems([FromQuery] GetLastAddedItemsQuery request) =>
            _mediator.Send(request, HttpContext.RequestAborted);

        [HttpGet("{id}")]
        public Task<ItemVm> GetItem(Guid id) =>
            _mediator.Send(new GetItemQuery(id, HttpContext.User.Identity?.Name), HttpContext.RequestAborted);

        [HttpPut("edit")]
        [Authorize]
        public void EditItem([FromBody] EditItemCommand request)
        {
            request.CurrentUserId = HttpContext.User.Identity?.Name;
            
            _mediator.Send(request, HttpContext.RequestAborted);
        }

        [HttpPut("like/{id}")]
        [Authorize]
        public Task LikeItem(Guid id) =>
            _mediator.Send(new LikeItemQuery(id, HttpContext.User.Identity?.Name), HttpContext.RequestAborted);

        [HttpDelete("{id}")]
        [Authorize]
        public Task RemoveItem(Guid id) => 
            _mediator.Send(new RemoveItemCommand(id), HttpContext.RequestAborted);

		[HttpPost("cover")]
		[Authorize]
        public async Task<string?> UploadCover()
        {
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.First();

            return await _mediator.Send(new UploadFileCommand(file), HttpContext.RequestAborted);
        }

		[HttpGet("search-tags/{query}")]
		public Task<List<TagVm>> SearchTags(string? query) => 
            _mediator.Send(new SearchTagsQuery(query ?? string.Empty));

		[HttpGet("top-tags")]
		public Task<List<TagVm>> GetTopTags() => 
            _mediator.Send(new GetTopTagsQuery(), HttpContext.RequestAborted);
	}
}