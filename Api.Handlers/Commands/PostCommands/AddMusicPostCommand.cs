﻿namespace Api.Handlers.Commands.PostCommands;

public class AddMusicPostCommand : HandlerBase, Command<Task<Post>, AddMusicDto>
{
    private readonly IContainer container;
    
    public AddMusicPostCommand(ModelsContext context, IContainer container) : base(context)
    {
        this.container = container;
    }

    public async Task<Post> Handle(AddMusicDto message)
    {
        Post post = await this.context.Posts
            .FirstOrDefaultAsync(p => p.Id.Equals(message.PostId));

        if (post == null)
        {
            throw new HandlerException(ErrorType.PostNotFound);
        }
        
        if (message.File == null || message.File.Length == 0)
        {
            throw new HandlerException(ErrorType.Undefined);
        }
        
        if (!DataValidator.IsSoundFormatSupported(message.File.FileName))
        {
            throw new HandlerException(ErrorType.WrongFormatFile);
        }

        using (var ms = new MemoryStream())
        {
            message.File.CopyTo(ms);
            post.MusicUrl = this.container.GetMusicUrl(ms.ToArray(), post.Id, message.File.FileName).Result;
        }

        await this.context.SaveChangesAsync();

        return post;
    }
}
