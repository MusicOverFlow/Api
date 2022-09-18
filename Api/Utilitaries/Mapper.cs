using AutoMapper;

namespace Api.Utilitaries;

public abstract class Mapper
{
    private static readonly IMapper m = new MapperConfiguration(c =>
    {
        c.CreateMap<Account, AccountResource>();
        c.CreateMap<Post, PostResource>();
        c.CreateMap<Commentary, CommentaryResource>();
        c.CreateMap<Group, GroupResource>();
    }).CreateMapper();

    public static AccountResource AccountToResource(Account a) => m.Map<AccountResource>(a);
    public static PostResource PostToResource(Post p) => m.Map<PostResource>(p);
    public static CommentaryResource CommentaryToResource(Commentary c) => m.Map<CommentaryResource>(c);
    public static GroupResource GroupToResource(Group g) => m.Map<GroupResource>(g);
}
