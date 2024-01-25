using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/group-zalos")]
    [ApiController]
    public class ZaloGroupController : ControllerBase
    {
        private readonly INhomZaloService _nhomZaloService;
        private readonly ILogger<ZaloGroupController> _logger;

        public ZaloGroupController(INhomZaloService nhomZaloService, ILogger<ZaloGroupController> logger)
        {
            _nhomZaloService = nhomZaloService;
            _logger = logger;
        }


        // Manage ZaloGroup
        [HttpGet("get")]
        public async Task<ActionResult<List<NhomZalo>>> GetAllZaloGroupsAsync()
        {
            try
            {
                var groups = await _nhomZaloService.GetAllZaloAsync();
                return Ok(groups);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllZaloGroupsAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching Zalo groups.");
            }
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<NhomZalo>> GetZaloGroupByIdAsync(string id)
        {
            try
            {
                var group = await _nhomZaloService.GetGroupByIdAsync(id);

                if (group == null)
                    return NotFound($"Zalo group with ID {id} not found.");

                return Ok(group);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetZaloGroupByIdAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching Zalo group.");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateZaloGroupAsync([FromBody] NhomZalo zalo)
        {
            try
            {
                await _nhomZaloService.AddNewZaloAsync(zalo);
                return CreatedAtAction(nameof(GetZaloGroupByIdAsync), new { id = zalo.Id }, zalo);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateZaloGroupAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating Zalo group.");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateZaloGroupAsync(string id, [FromBody] NhomZalo zalo)
        {
            try
            {
                await _nhomZaloService.UpdateZaloAsync(id, zalo);
                return Ok("Zalo group updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateZaloGroupAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating Zalo group.");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteZaloGroupAsync(string id)
        {
            try
            {
                await _nhomZaloService.DeleteZaloAsync(id);
                return Ok("Zalo group deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteZaloGroupAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting Zalo group.");
            }
        }

        // Manage User in ZaloGroup methods
        [HttpGet("get/{nhomZaloId}/users")]
        public async Task<ActionResult<List<UserNhomZalo>>> GetUsersInZaloGroupAsync(string nhomZaloId)
        {
            try
            {
                var users = await _nhomZaloService.GetUsersInGroupAsync(nhomZaloId);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetUsersInZaloGroupAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching users in Zalo group.");
            }
        }

        [HttpPost("add-user-to-zalo/{nhomZaloId}/users")]
        public async Task<ActionResult> AddUserToZaloGroupAsync(string nhomZaloId, [FromBody] UserNhomZalo user)
        {
            try
            {
                await _nhomZaloService.AddUserToGroupAsync(nhomZaloId, user);
                return Ok($"User added to Zalo group {nhomZaloId} successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddUserToZaloGroupAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding user to Zalo group.");
            }
        }

        [HttpGet("get-user-in-zalo-group/{nhomZaloId}/users/{userId}")]
        public async Task<ActionResult<UserNhomZalo>> GetUserInZaloGroupAsync(string nhomZaloId, string userId)
        {
            try
            {
                var user = await _nhomZaloService.GetUserInGroupAsync(nhomZaloId, userId);

                if (user == null)
                    return NotFound($"User with ID {userId} not found in Zalo group {nhomZaloId}.");

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetUserInZaloGroupAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching user in Zalo group.");
            }
        }

        [HttpPut("update-user-in-zalo-group/{nhomZaloId}/users")]
        public async Task<ActionResult> UpdateUserInZaloGroupAsync(string nhomZaloId, [FromBody] UserNhomZalo updatedUser)
        {
            try
            {
                await _nhomZaloService.UpdateUserInGroupAsync(nhomZaloId, updatedUser);
                return Ok($"User in Zalo group {nhomZaloId} updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateUserInZaloGroupAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating user in Zalo group.");
            }
        }

        [HttpDelete("delete-user-in-zalo-group{nhomZaloId}/users/{userId}")]
        public async Task<ActionResult> RemoveUserFromZaloGroupAsync(string nhomZaloId, string userId)
        {
            try
            {
                await _nhomZaloService.RemoveUserFromGroupAsync(nhomZaloId, userId);
                return Ok($"User removed from Zalo group {nhomZaloId} successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in RemoveUserFromZaloGroupAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while removing user from Zalo group.");
            }
        }
    }
}
