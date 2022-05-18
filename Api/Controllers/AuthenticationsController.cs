using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Api.Models;
using Api.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Api.Controllers;

[Route("api/authenticate")]
[ApiController]
public class AuthenticationsController : ControllerBase
{
    private readonly ModelsContext context;
    private readonly IConfiguration configuration;

    public AuthenticationsController(ModelsContext context, IConfiguration configuration)
    {
        this.context = context;
        this.configuration = configuration;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Authenticate(Authentication connection)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(connection.MailAddress));

        if (account == null || !this.IsPasswordCorrect(connection.Password, account.PasswordHash, account.PasswordSalt))
        {
            return BadRequest(new { errorMessage = "Wrong credentials" });
        }

        string jwt = this.CreateJwt(account);

        return Ok(new { jwt = jwt });
    }

    private bool IsPasswordCorrect(string password, byte[] hash, byte[] salt)
    {
        return new HMACSHA512(salt)
            .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password))
            .SequenceEqual(hash);
    }

    private string CreateJwt(Account account)
    {
        DateTime expiration = DateTime.Now.AddMinutes(Convert.ToDouble(this.configuration.GetSection("AppSettings:Token:ExpirationTimeInMinutes").Value));

        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
            claims: new List<Claim>()
            {
                new Claim(ClaimTypes.Email, account.MailAddress),
                new Claim(ClaimTypes.Role, account.Role),
            },
            expires: expiration,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(this.configuration.GetSection("AppSettings:Token:Key").Value)),
                SecurityAlgorithms.HmacSha256Signature
            )
        ));
    }
}
