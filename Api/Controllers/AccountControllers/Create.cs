using Azure.Storage.Blobs;
using System.IO;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;

namespace Api.Controllers.AccountControllers;

public partial class AccountController
{
    [HttpPost]
    public async Task<ActionResult<AccountResource>> Create(CreateAccountRequest request)
    {
        if (!this.dataValidator.IsMailAddressValid(request.MailAddress))
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.InvalidMail));
        }

        if (!this.dataValidator.IsPasswordValid(request.Password))
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.InvalidPassword));
        }

        bool isMailAlreadyInUse = await this.context.Accounts
            .AnyAsync(a => a.MailAddress.Equals(request.MailAddress));

        if (isMailAlreadyInUse)
        {
            return BadRequest(this.exceptionHandler.GetError(ErrorType.MailAlreadyInUse));
        }

        this.EncryptPassword(request.Password, out byte[] hash, out byte[] salt);
        
        Account account = new Account()
        {
            MailAddress = request.MailAddress.Trim(),
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = Role.Admin.ToString(),
            Firstname = request.Firstname ?? "Unknown",
            Lastname = request.Lastname ?? "Unknown",
            Pseudonym = request.Pseudonym ?? "Anonymous",
            ProfilPicUrl = this.GetProfilPicUrl(request.ProfilPic, request.MailAddress.Trim()).Result,
            CreatedAt = DateTime.Now,
        };

        this.context.Accounts.Add(account);
        
        await this.context.SaveChangesAsync();
        
        return Created(nameof(Create), this.mapper.Account_ToResource(account));
    }

    private void EncryptPassword(string password, out byte[] hash, out byte[] salt)
    {
        using HMACSHA512 hmac = new HMACSHA512();

        hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        salt = hmac.Key;
    }

    async private Task<string> GetProfilPicUrl(byte[] profilPic, string mailAddress)
    {
        if (profilPic != null)
        {
            BlobContainerClient blobContainer = new BlobContainerClient(
                this.configuration.GetSection("ConnectionStrings:MusicOverflowStorageAccount").Value,
                "profil-pics"
            );

            BlobClient blobClient = blobContainer.GetBlobClient(mailAddress + ".profilpic.png");
            await blobClient.UploadAsync(new BinaryData(profilPic), true);
            
            return blobClient.Uri.AbsoluteUri.Replace("%40", "@");
        }
        else
        {
            return "https://musicoverflowstorage.blob.core.windows.net/profil-pics/placeholder.png";
        }
    }
}
