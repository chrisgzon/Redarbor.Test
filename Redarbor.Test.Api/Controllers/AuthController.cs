using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Redarbor.Test.Application.DTOs;
using Redarbor.Test.Application.Interfaces;
using System.Security.Claims;

namespace Redarbor.Test.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Login endpoint - Returns JWT token
        /// </summary>
        /// <param name="request">Login credentials</param>
        /// <returns>JWT token and refresh token</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            _logger.LogInformation("Login attempt for username: {Username}", request.Username);

            try
            {
                var response = await _authService.LoginAsync(request.Username, request.Password);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Failed login attempt for username: {Username}", request.Username);
                return Unauthorized(new { message = "Invalid credentials" });
            }
        }

        /// <summary>
        /// Refresh token endpoint
        /// </summary>
        /// <param name="request">Refresh token</param>
        /// <returns>New JWT token</returns>
        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            _logger.LogInformation("Token refresh attempt");

            try
            {
                var response = await _authService.RefreshTokenAsync(request.RefreshToken);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Failed token refresh");
                return Unauthorized(new { message = "Invalid refresh token" });
            }
        }

        /// <summary>
        /// Validate token endpoint
        /// </summary>
        [HttpPost("validate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ValidateToken([FromBody] ValidateTokenRequestDto request)
        {
            var isValid = await _authService.ValidateTokenAsync(request.Token);

            if (isValid)
                return Ok(new { valid = true });

            return Unauthorized(new { valid = false });
        }

        /// <summary>
        /// Get current user info
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst("id")?.Value;
            var username = User.FindFirst("username")?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            return Ok(new
            {
                id = userId,
                username,
                email
            });
        }
    }
}
