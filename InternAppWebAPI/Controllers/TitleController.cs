using InternAppWebAPI.Models;
using InternAppWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternAppWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitleController : ControllerBase
    {
        private readonly TitleService _titleService;

        public TitleController(TitleService titleService)
        {
            _titleService = titleService;
        }

        [HttpGet]
        public async Task<ActionResult> getTitles()
        {
            if(_titleService.titles != null) return Ok(_titleService.titles);
            try
            {
                await _titleService.getTitles();
                return Ok(_titleService.titles);
            }
            catch (Exception error) {
                return StatusCode(400, error.Message);
            }

        }


    }
}
