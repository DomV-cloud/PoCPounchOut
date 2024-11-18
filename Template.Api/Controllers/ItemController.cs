using Microsoft.AspNetCore.Mvc;
using Template.Api.Filters;
using Template.Application.Common.Interfaces.Persistance;

namespace Template.Api.Controllers
{
    [ApiController]
    [Route("item")]
    [ErrorHandlingFilter]
    public class ItemController : Controller
    {
        private readonly IItemRepository _itemRepository;

        public ItemController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet("all", Name = "all")]
        public async Task<IActionResult> GetAllItems()
        {
            var response = await _itemRepository.GetAllItems();

            if (response is null)
            {
                return BadRequest();
            }

            return Ok(response);
        }
    }
}
