using Microsoft.AspNetCore.Mvc;
using TrainComponentManagement.BLL.DTOs;
using TrainComponentManagement.BLL.Services.Interfaces;

namespace TrainComponentManagement.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentController : ControllerBase
    {
        private readonly IComponentService _componentService;

        public ComponentController(IComponentService componentService)
        {
            _componentService = componentService;
        }

        /// <summary>
        /// Get all components.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComponentDto>>> GetAll()
        {
            var items = await _componentService.GetAllAsync();
            return Ok(items);
        }

        /// <summary>
        /// Get a single component by Id.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ComponentDto>> Get(int id)
        {
            var item = await _componentService.GetAsync(id);
            return Ok(item);
        }

        /// <summary>
        /// Create a new component.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ComponentDto>> Create(
            [FromBody] CreateOrUpdateComponentDto dto,
            [FromHeader(Name = "Idempotency-Key")] string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return BadRequest("Idempotency-Key header is required.");

            var created = await _componentService.CreateAsync(dto, key);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Update an existing component.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] CreateOrUpdateComponentDto dto)
        {
            try
            {
                await _componentService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Delete a component by Id.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _componentService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Bulk insert several components at once.
        /// </summary>
        [HttpPost("bulk")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkInsert(
            [FromBody] IEnumerable<CreateOrUpdateComponentDto> dtos,
            [FromHeader(Name = "Idempotency-Key")] string idempotencyKey)
        {
            if (dtos == null || !dtos.Any())
                return BadRequest("No components specified.");

            await _componentService.BulkInsertAsync(dtos, idempotencyKey);
            return NoContent();
        }

        /// <summary>
        ///  Bulk delete several components at once.
        /// </summary>
        [HttpPost("bulk-delete")]
        public async Task<IActionResult> BulkDelete(
            [FromBody] IEnumerable<int> ids,
            [FromHeader(Name = "Idempotency-Key")] string key)
        {
            if (ids == null || !ids.Any())
                return BadRequest("No IDs specified.");

            await _componentService.BulkDeleteAsync(ids, key);
            return NoContent();
        }

    }
}
