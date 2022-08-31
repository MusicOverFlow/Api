using Api.Handlers.Dtos;
using Api.Handlers.Kernel;
using Api.Handlers.Utilitaries;
using Api.Models;
using Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Api.Handlers.Queries.Authentication;

public class AuthenticationQuery : HandlerBase, Query<Task<string>, AuthenticationDto>
{
    public IConfiguration Configuration { get; set; }

    public AuthenticationQuery(ModelsContext context) : base(context)
    {

    }

    public async Task<string> Handle(AuthenticationDto authenticationRequest)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(authenticationRequest.MailAddress));

        if (account == null || !IsPasswordCorrect(authenticationRequest.Password, account.PasswordHash, account.PasswordSalt))
        {
            throw new HandlerException(ErrorType.WrongCredentials);
        }

        return this.CreateJwt(account);
    }

    private bool IsPasswordCorrect(string password, byte[] hash, byte[] salt)
    {
        return new HMACSHA512(salt)
            .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password))
            .SequenceEqual(hash);
    }

    private string CreateJwt(Account account)
    {
        DateTime expiration = DateTime.Now.AddMinutes(Convert.ToDouble(this.Configuration.GetSection("AppSettings:Token:ExpirationTimeInMinutes").Value));

        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
            claims: new List<Claim>()
            {
                new Claim(ClaimTypes.Email, account.MailAddress),
                new Claim(ClaimTypes.Role, account.Role),
            },
            expires: expiration,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(this.Configuration.GetSection("AppSettings:Token:Key").Value)),
                SecurityAlgorithms.HmacSha256Signature
            )
        ));
    }
}
