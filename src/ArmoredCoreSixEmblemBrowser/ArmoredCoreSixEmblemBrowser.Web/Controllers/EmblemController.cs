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
            if (!string.IsNullOrEmpty(name) || platforms != null)
            {
                // search by name or platforms    
            }

            var emblems = await _emblemService.GetEmblems(page, totalPerPage);
            
            return Ok(emblems);
        }
    }
}
