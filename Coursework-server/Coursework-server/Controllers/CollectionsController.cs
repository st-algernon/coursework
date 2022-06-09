#nullable enable
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
	public class CollectionsController : ControllerBase
	{
        private readonly IMediator _mediator;
        
		public CollectionsController(IMediator mediator)
		{
            _mediator = mediator;
        }

        [HttpPost("create")]
        [Authorize]
        public void CreateCollection([FromBody] CreateCollectionCommand request)
        {
            request.CurrentUserId = HttpContext.User.Identity?.Name;
            
            _mediator.Send(request, HttpContext.RequestAborted);
        }

		[HttpGet("{id}")]
		public Task<CollectionVm> GetCollection(Guid id) => 
            _mediator.Send(new GetCollectionQuery(id), HttpContext.RequestAborted);

        [HttpGet("short/{id}")]
        public Task<ShortCollectionVm> GetShortCollection(Guid id) => 
            _mediator.Send(new GetShortCollectionQuery(id), HttpContext.RequestAborted);

        [HttpGet("owner/{userId}")]
        public Task<List<ShortCollectionVm>> GetUserCollections(Guid userId) => 
            _mediator.Send(new GetUserCollectionsQuery(userId), HttpContext.RequestAborted);

		[HttpGet("largest")]
		public Task<List<ShortCollectionVm>> GetLargestCollections() => 
            _mediator.Send(new GetLargestCollectionsQuery(), HttpContext.RequestAborted);

        [HttpPut("edit")]
        [Authorize]
        public void EditCollection([FromBody] EditCollectionCommand request)
        {
            request.CurrentUserId = HttpContext.User.Identity?.Name;
            
            _mediator.Send(request, HttpContext.RequestAborted);
        }

		[HttpDelete("{id}")]
		[Authorize]
		public Task RemoveCollection(Guid id) => 
            _mediator.Send(new RemoveCollectionCommand(id), HttpContext.RequestAborted);

		[HttpPost("cover")]
		[Authorize]
		public Task<string> UploadCover(IFormFile file) =>
			_mediator.Send(new UploadFileCommand(file), HttpContext.RequestAborted);

        [HttpGet("{id}/fields")]
        public Task<List<FieldWithTypeNameVm>> GetCollectionFields(Guid id) =>
            _mediator.Send(new GetCollectionFieldsQuery(id), HttpContext.RequestAborted);

        [HttpGet("{id}/tags")]
        public Task<List<TagVm>> GetCollectionTags(Guid id) =>
            _mediator.Send(new GetCollectionTagsQuery(id), HttpContext.RequestAborted);

		[HttpGet("field-types")]
		public Task<List<FieldTypeVm>> GetFieldTypes() => 
            _mediator.Send(new GetFieldTypesQuery(), HttpContext.RequestAborted);
	}
}