namespace Api.Handlers.Commands.GroupCommands;

public class CreateGroupCommand : HandlerBase, Command<Task<Group>, CreateGroupDto>
{
    public CreateGroupCommand(ModelsContext context) : base(context)
    {
        
    }

    public async Task<Group> Handle(CreateGroupDto message)
    {
        Account account = await this.context.Accounts
            .FirstOrDefaultAsync(a => a.MailAddress.Equals(message.CreatorMailAddress));

        if (account == null)
        {
            throw new HandlerException(ErrorType.AccountNotFound);
        }

        if (string.IsNullOrWhiteSpace(message.Name))
        {
            throw new HandlerException(ErrorType.GroupeMissingName);
        }

        byte[] fileBytes = null;
        if (message.GroupPic != null && message.GroupPic.Length > 0)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                message.GroupPic.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
        }

        Guid id = Guid.NewGuid();
        Group group = new Group()
        {
            Id = id,
            Name = message.Name,
            Description = message.Description,
            PicUrl = Blob.GetGroupPicUrl(fileBytes, id).Result,
            CreatedAt = DateTime.Now,

            Owner = account,
        };

        this.context.Groups.Add(group);
        account.Groups.Add(group);

        await this.context.SaveChangesAsync();

        return group;
    }
}
