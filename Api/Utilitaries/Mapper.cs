using Api.Models.ExpositionModels.Resources;
using AutoMapper;

namespace Api.Utilitaries;

public abstract class Mapper
{
    private static readonly IMapper m = new MapperConfiguration(c =>
    {
        c.CreateMap<Account, AccountResource>();
        c.CreateMap<Account, AccountResource_WithPosts>();
        c.CreateMap<Account, AccountResource_WithPosts_AndGroups>();
        c.CreateMap<Account, AccountResource_WithPosts_AndGroups_AndFollows>();

        c.CreateMap<Post, PostResource>();

        c.CreateMap<Commentary, CommentaryResource>();
        c.CreateMap<Commentary, CommentaryResource_WithPost>();

        c.CreateMap<Group, GroupResource>();
        c.CreateMap<Group, GroupResource_WithMembers>();
        c.CreateMap<Group, GroupResource_WithPosts>();
        c.CreateMap<Group, GroupResource_WithMembers_AndPosts>();
    }).CreateMapper();

    public static AccountResource Account_ToResource(Account a) => m.Map<AccountResource>(a);
    public static AccountResource_WithPosts Account_ToResource_WithPosts(Account a) => m.Map<AccountResource_WithPosts>(a);
    public static AccountResource_WithPosts_AndGroups Account_ToResource_WithPosts_AndGroups(Account a) => m.Map<AccountResource_WithPosts_AndGroups>(a);
    public static AccountResource_WithPosts_AndGroups_AndFollows Account_ToResource_WithPosts_AndGroups_AndFollows(Account a) => m.Map<AccountResource_WithPosts_AndGroups_AndFollows>(a);

    public static PostResource Post_ToResource(Post p) => m.Map<PostResource>(p);

    public static CommentaryResource Commentary_ToResource(Commentary c) => m.Map<CommentaryResource>(c);
    public static CommentaryResource_WithPost Commentary_ToResource_WithPost(Commentary c) => m.Map<CommentaryResource_WithPost>(c);

    public static GroupResource Group_ToResource(Group g) => m.Map<GroupResource>(g);
    public static GroupResource_WithMembers Group_ToResource_WithMembers(Group g) => m.Map<GroupResource_WithMembers>(g);
    public static GroupResource_WithPosts Group_ToResource_WithPosts(Group g) => m.Map<GroupResource_WithPosts>(g);
    public static GroupResource_WithMembers_AndPosts Group_ToResource_WithMembers_AndPosts(Group g) => m.Map<GroupResource_WithMembers_AndPosts>(g);
}
