using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Api.Models.Entities;
using Api.Models.Enums;
using Api.Utilitaries;
using System.Security.Cryptography;
using static Api.Wrappers.AuthorizeRolesAttribute;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly ModelsContext context;

    public AccountsController(ModelsContext context)
    {
        this.context = context;
    }

    [HttpPost]
    public async Task<ActionResult<AccountResource>> Create(CreateAccount request)
    {
        if (!DataValidator.IsMailAddressValid(request.MailAddress))
        {
            return BadRequest(new { message = "Invalid mail address" });
        }

        if (!DataValidator.IsPasswordValid(request.Password))
        {
            return BadRequest(new { message = "Invalid password" });
        }

        bool isMailAlreadyInUse = await this.context.Accounts
            .AnyAsync(a => a.MailAddress.Equals(request.MailAddress));

        if (isMailAlreadyInUse)
        {
            return BadRequest(new { message = "Mail already in use" });
        }

        this.EncryptPassword(request.Password, out byte[] hash, out byte[] salt);

        Account account = new Account()
        {
            MailAddress = request.MailAddress.Trim(),
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = Role.Admin.ToString(),
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            CreatedAt = DateTime.Now,
            Posts = new List<Post>(),
            Commentaries = new List<Commentary>(),
        };

        this.context.Accounts.Add(account);

        await this.context.SaveChangesAsync();
        
        return Created(nameof(Create), Mapper.AccountToResource(account));
    }

    [HttpGet, AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<List<AccountResource>>> Read(string mailAddress = null)
    {
        IQueryable<Account> query = this.context.Accounts
            .Include(a => a.Posts)
                .ThenInclude(p => p.Commentaries)
            .Include(a => a.Commentaries);

        if (!string.IsNullOrWhiteSpace(mailAddress))
        {
            query = query.Where(a => a.MailAddress.Equals(mailAddress));
        }

        List<AccountResource> accounts = new List<AccountResource>();

        await query.ForEachAsync(a => accounts.Add(Mapper.AccountToResourceWithPostsAndCommentaries(a)));

        if (!string.IsNullOrWhiteSpace(mailAddress) && accounts.Count == 0)
        {
            return NotFound(new { message = "Account not found" });
        }

        return Ok(accounts);
    }

    [HttpGet("self"), AuthorizeEnum(Role.User, Role.Moderator, Role.Admin)]
    public async Task<ActionResult<AccountResource>> ReadSelf()
    {
        string mailAddress = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email)).Value;

        Account account = await this.context.Accounts
            .Where(a => a.MailAddress.Equals(mailAddress))
            .Include(a => a.Posts)
                .ThenInclude(p => p.Commentaries)
            .Include(a => a.Commentaries)
            .FirstOrDefaultAsync();

        if (account == null)
        {
            return NotFound(new { message = "Account not found" });
        }

        return Ok(Mapper.AccountToResourceWithPostsAndCommentaries(account));
    }

    [HttpPut("role"), AuthorizeEnum(Role.Admin)]
    public async Task<ActionResult<AccountResource>> UpdateRole(string mailAddress, string role)
    {
        if (string.IsNullOrWhiteSpace(mailAddress) || string.IsNullOrWhiteSpace(role))
        {
            return BadRequest(new { message = "Malformed request" });
        }

        Account account = await this.context.Accounts
            .Where(a => a.MailAddress.Equals(mailAddress))
            .FirstOrDefaultAsync();

        if (account == null)
        {
            return NotFound(new { message = "Account not found" });
        }
        
        string newRole = RoleParser.Handle(role);
        
        if (string.IsNullOrWhiteSpace(newRole))
        {
            return BadRequest(new { message = $"Role {role} does not exist" });
        }

        account.Role = newRole;

        await this.context.SaveChangesAsync();
        
        return Ok(Mapper.AccountToResource(account));
    }

    [HttpDelete, AuthorizeEnum(Role.Admin)]
    public async Task<ActionResult> Delete(string mailAddress)
    {
        Account account = await this.context.Accounts
            .Include(a => a.Posts)
            .Include(a => a.Commentaries)
            .Where(a => a.MailAddress.Equals(mailAddress))
            .FirstOrDefaultAsync();

        if (account == null)
        {
            return NotFound(new { message = "Account not found" });
        }
        
        this.context.Accounts.Remove(account);

        await this.context.SaveChangesAsync();

        return Ok();
    }

    private void EncryptPassword(string password, out byte[] hash, out byte[] salt)
    {
        using HMACSHA512 hmac = new HMACSHA512();
        
        hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        salt = hmac.Key;
    }
}
