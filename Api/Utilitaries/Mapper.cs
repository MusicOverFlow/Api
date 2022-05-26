using AutoMapper;

namespace Api.Utilitaries;

public class Mapper
{
    private readonly IMapper m;

    public Mapper()
    {
        MapperConfiguration configuration = new MapperConfiguration(c =>
        {
            c.CreateMap<Account, AccountResource>();
            c.CreateMap<Account, AccountResource_WithGroups>();
            c.CreateMap<Account, AccountResource_WithPosts>();
            c.CreateMap<Account, AccountResource_WithPosts_AndGroups>();

            c.CreateMap<Post, PostResource>();
            c.CreateMap<Post, PostResource_WithCommentaries_AndLikes>();

            c.CreateMap<Commentary, CommentaryResource>();
            c.CreateMap<Commentary, CommentaryResource_WithPost>();

            c.CreateMap<Group, GroupResource>();
            c.CreateMap<Group, GroupResource_WithMembers>();
            c.CreateMap<Group, GroupResource_WithPosts>();
            c.CreateMap<Group, GroupResource_WithMembers_AndPosts>();
        });

        this.m = configuration.CreateMapper();
    }

    public AccountResource Account_ToResource(Account a) => m.Map<AccountResource>(a);
    public AccountResource_WithGroups Account_ToResource_WithGroups(Account a) => m.Map<AccountResource_WithGroups>(a);
    public AccountResource_WithPosts Account_ToResource_WithPosts(Account a) => m.Map<AccountResource_WithPosts>(a);
    public AccountResource_WithPosts_AndGroups Account_ToResource_WithGroups_AndPosts(Account a) => m.Map<AccountResource_WithPosts_AndGroups>(a);

    public PostResource Post_ToResource(Post p) => m.Map<PostResource>(p);
    public PostResource_WithCommentaries_AndLikes Post_ToResource_WithCommentaries_AndLikes(Post p) => m.Map<PostResource_WithCommentaries_AndLikes>(p);

    public CommentaryResource Commentary_ToResource(Commentary c) => m.Map<CommentaryResource>(c);
    public CommentaryResource_WithPost Commentary_ToResource_WithPost(Commentary c) => m.Map<CommentaryResource_WithPost>(c);

    public GroupResource Group_ToResource(Group g) => m.Map<GroupResource>(g);
    public GroupResource_WithMembers Group_ToResource_WithMembers(Group g) => m.Map<GroupResource_WithMembers>(g);
    public GroupResource_WithPosts Group_ToResource_WithPosts(Group g) => m.Map<GroupResource_WithPosts>(g);
    public GroupResource_WithMembers_AndPosts Group_ToResource_WithMembers_AndPosts(Group g) => m.Map<GroupResource_WithMembers_AndPosts>(g);
}
