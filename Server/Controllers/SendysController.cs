using BlazorAuth.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Services.Sendys;
using Server.Entities.Constants;

namespace BlazorAuth.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize(Policy = Policy.Sendys)]
    public class SendysController : ControllerBase
    {
        private readonly ISendysService _sendysService;
        public SendysController(ISendysService sendysService)
        {
            _sendysService = sendysService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SaveStrings(IEnumerable<StringValueDto> stringValueDtos)
        {
            try
            {
                if (stringValueDtos != null && stringValueDtos.Any())
                {
                    var result = await _sendysService.SaveStrings(stringValueDtos);
                    return Ok(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("[action]/{userEmail}")]
        public async Task<IActionResult> GetUserDetails(string userEmail)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(userEmail))
                {
                    var result = await _sendysService.GetUserDetails(userEmail);
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("[action]/{userEmail}")]
        public async Task<IActionResult> GetAllStringValues(string userEmail)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(userEmail))
                {
                    var result = await _sendysService.GetAllStringValues(userEmail);
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("[action]/{userEmail}")]
        public async Task<IActionResult> GetUserId(string userEmail)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(userEmail))
                {
                    var result = await _sendysService.GetUserId(userEmail);
                    return Ok(result != null ? new UserDetailDto(userId: result) : null);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet("[action]")]
        //[Authorize(Policy = Policy.Admin)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _sendysService.GetAllUsers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }










    }
}