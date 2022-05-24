using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Api.Controllers.AuthenticationControllers;

[Route("api/authenticate")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly ModelsContext context;
    private readonly IConfiguration configuration;

    public AuthenticateController(ModelsContext context, IConfiguration configuration)
    {
        this.context = context;
        this.configuration = configuration;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Authenticate(Authentication request)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(request.MailAddress));

        if (account == null || !IsPasswordCorrect(request.Password, account.PasswordHash, account.PasswordSalt))
        {
            return BadRequest(new { message = "Wrong credentials" });
        }

        string jwt = CreateJwt(account);

        return Ok(new { jwt });
    }

    private bool IsPasswordCorrect(string password, byte[] hash, byte[] salt)
    {
        return new HMACSHA512(salt)
            .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password))
            .SequenceEqual(hash);
    }

    private string CreateJwt(Account account)
    {
        DateTime expiration = DateTime.Now.AddMinutes(Convert.ToDouble(configuration.GetSection("AppSettings:Token:ExpirationTimeInMinutes").Value));

        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
            claims: new List<Claim>()
            {
                new Claim(ClaimTypes.Email, account.MailAddress),
                new Claim(ClaimTypes.Role, account.Role),
            },
            expires: expiration,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token:Key").Value)),
                SecurityAlgorithms.HmacSha256Signature
            )
        ));
    }
}
