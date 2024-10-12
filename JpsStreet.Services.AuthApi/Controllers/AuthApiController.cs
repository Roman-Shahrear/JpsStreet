using JpsStreet.Services.AuthApi.Models.DTo;
using JpsStreet.Services.AuthApi.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace JpsStreet.Services.AuthApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDTo _response;

        public AuthApiController(IAuthService authService)
        {
            _authService = authService;
            _response = new();
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTo model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTo model)
        {
            var loginResponse = await _authService.Login(model);
            if(loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "UserName or Password is incorrect";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("assaignRole")]
        public async Task<IActionResult> AssaignRole([FromBody] RegistrationRequestDTo model)
        {
            var assaignRoleSuccessful = await _authService.AssaignRole(model.Email,model.Role.ToUpper());
            if (!assaignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encounter";
                return BadRequest(_response);
            }
            return Ok(_response);
        }
    }
}
