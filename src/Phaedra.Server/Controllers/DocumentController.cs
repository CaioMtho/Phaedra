using Microsoft.AspNetCore.Mvc;
using Phaedra.Server.Models.Utilities;
using Phaedra.Server.Services;
using System.Linq.Expressions;
using Phaedra.Server.Models.Entities.Documents;
using Microsoft.AspNetCore.JsonPatch;

namespace Phaedra.Server.Controllers
{
    [Route("project/{projectId}/[controller]")]
    [ApiController]
    public class DocumentController(IDocumentService<Document> documentService) : ControllerBase
    {
        private readonly IDocumentService<Document> _documentService = documentService;

        [HttpGet]
        public async Task<ActionResult<PaginatedList<Document>>> GetDocumentAsync(
           Expression<Func<Document, bool>>? filter = null,
           Func<IQueryable<Document>, IOrderedQueryable<Document>>? orderBy = null,
           int? page = null,
           int? pageSize = null)
        {
            var items = await _documentService.GetAsync(filter, orderBy, page, pageSize);
            var paginatedList = new PaginatedList<Document>(items.AsQueryable(), page ?? 1, pageSize ?? 10);
            return Ok(paginatedList);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Document>> GetDocumentAsync(int projectId, int id)
        {
            var document = await _documentService.GetAsync(d => d.Id == id && d.ProjectId == projectId) ?? throw new KeyNotFoundException($"Document with id {id} not found");
            return Ok(document);
        }

        [HttpPost]
        public async Task<ActionResult<Document>> PostDocumentAsync([FromBody] Document document)
        {
            ArgumentNullException.ThrowIfNull(document);
            await _documentService.AddAsync(document);
            return CreatedAtAction(nameof(GetDocumentAsync), new { id = document.Id }, document);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Document>> PatchDocumentAsync(int id, [FromBody] JsonPatchDocument<Document> patch)
        {
            return Ok(await _documentService.UpdateAsync(id, patch));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDocumentAsync(int id)
        {
            await _documentService.DeleteAsync(id);
            return NoContent();
        }

    }
}
