using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _autService;

        public AuthController(IAuthService autService)
        {
            _autService = autService;
        }
        [HttpPost("registerSecondAccount")]
        public IActionResult RegisterSecondAccount(UserForRegister userForRegister, int companyId)
        {

            var userExists = _autService.UserExists(userForRegister.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }
            var registerResult = _autService.RegisterSecondAccount(userForRegister, userForRegister.Password);
            var result = _autService.CreateAccessToken(registerResult.Data, 0);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(registerResult.Message);
        }
        [HttpPost("register")]
        public IActionResult Register(UserAndCompanyRegister userAndCompanyRegister)
        {
            
            var userExists = _autService.UserExists(userAndCompanyRegister.UserForRegister.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }
            var companyExist = _autService.CompanyExists(userAndCompanyRegister.Company); 
            
            if (!companyExist.Success)
            {
                return BadRequest(userExists.Message);
            }
            var registerResult = _autService.Register(userAndCompanyRegister.UserForRegister, userAndCompanyRegister.UserForRegister.Password, userAndCompanyRegister.Company);
            var result = _autService.CreateAccessToken(registerResult.Data,registerResult.Data.CompanyId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(registerResult.Message);
        }
        [HttpPost("login")]
        public IActionResult Login(UserForLogin userForLogin)
        {
            var userToLogin =_autService.Login(userForLogin);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }
            var result = _autService.CreateAccessToken(userToLogin.Data,0);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);

        }
    }
    
}
