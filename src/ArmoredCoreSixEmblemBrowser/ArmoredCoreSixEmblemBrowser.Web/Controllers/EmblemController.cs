using System.Net;
using ArmoredCoreSixEmblemBrowser.Domain;
using ArmoredCoreSixEmblemBrowser.Domain.Objects;
using ArmoredCoreSixEmblemBrowser.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ArmoredCoreSixEmblemBrowser.Web.Controllers
{
    [Route("api/emblem")]
    [ApiController]
    [Produces(System.Net.Mime.MediaTypeNames.Application.Json)]
    public class EmblemController : ControllerBase
    {
        private readonly IEmblemBrowserService _emblemService;

        public EmblemController(IEmblemBrowserService emblemService)
        {
            _emblemService = emblemService;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(Emblem), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddEmblem(Emblem emblem)
        {
            var created = await _emblemService.AddEmblem(emblem);
            return Created($"api/emblem/{created.Id}", created);
        }
        
        [HttpGet("search/{page}/{totalPerPage}")]
        [ProducesResponseType(typeof(EmblemSearchResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchEmblems(int page, int totalPerPage, string? name, [FromQuery]PlatformType[]? platforms)
        {
            if (!string.IsNullOrEmpty(name) || platforms?.Length > 0)
            {
                var filteredEmblems = await _emblemService
                    .GetFilteredEmblems(page, totalPerPage, name ?? string.Empty, platforms?.ToList() ?? new List<PlatformType>());
                return Ok(filteredEmblems);
            }

            var emblems = await _emblemService.GetEmblems(page, totalPerPage);
            
            return Ok(emblems);
        }

        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetImageForEmblem(int id)
        {
            var data = await _emblemService.GetEmblemImage(id);
            return File(data.ImageData, data.Extension);
        }

        [HttpGet("tags")]
        public async Task<IActionResult> GetAllTags()
        {
            return Ok(await _emblemService.GetAllTags());
        }
    }
}
