using BusinessObject;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SilverJewelry_VVT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IBranchAccountRepo _accountRepository;

        public JwtController(IConfiguration configuration, IBranchAccountRepo accountRepository)
        {
            _configuration = configuration;
            _accountRepository = accountRepository;
        }


        [HttpPost]
        public IActionResult generateToken([FromBody] LoginRequest loginRequest)
        {
            BranchAccount branchAccount = _accountRepository.GetBranchAccount(loginRequest.email, loginRequest.password);

            if (branchAccount == null)
            {
                return Unauthorized();
            }


            var claims = new List<Claim>
    {
        new Claim("Email", loginRequest.email),
        new Claim(ClaimTypes.Role, branchAccount.Role.ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
