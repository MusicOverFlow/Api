using AutoMapper;

namespace Api.Utilitaries;

public class Mapper
{
    private readonly IMapper mapper;

    public Mapper()
    {
        MapperConfiguration configuration = new MapperConfiguration(c =>
        {
            c.CreateMap<Account, AccountResource>();
            c.CreateMap<Account, AccountResource_WithGroups>();
            c.CreateMap<Account, AccountResource_WithPosts>();
            c.CreateMap<Account, AccountResource_WithPosts_AndGroups>();

            c.CreateMap<Post, PostResource>();

            c.CreateMap<Commentary, CommentaryResource>();
            c.CreateMap<Commentary, CommentaryResource_WithPost>();

            c.CreateMap<Group, GroupResource>();
            c.CreateMap<Group, GroupResource_WithMembers>();
            c.CreateMap<Group, GroupResource_WithPosts>();
            c.CreateMap<Group, GroupResource_WithMembers_AndPosts>();
        });

        this.mapper = configuration.CreateMapper();
    }

    public AccountResource Account_ToResource(Account account) => this.mapper.Map<AccountResource>(account);
    public AccountResource Account_ToResource_WithGroups(Account account) => this.mapper.Map<AccountResource_WithGroups>(account);
    public AccountResource Account_ToResource_WithPosts(Account account) => this.mapper.Map<AccountResource_WithPosts>(account);
    public AccountResource Account_ToResource_WithGroups_AndPosts(Account account) => this.mapper.Map<AccountResource_WithPosts_AndGroups>(account);

    public PostResource Post_ToResource(Post post) => this.mapper.Map<PostResource>(post);

    public CommentaryResource Commentary_ToResource(Commentary commentary) => this.mapper.Map<CommentaryResource>(commentary);
    public CommentaryResource Commentary_ToResource_WithPost(Commentary commentary) => this.mapper.Map<CommentaryResource_WithPost>(commentary);

    public GroupResource Group_ToResource(Group group) => this.mapper.Map<GroupResource>(group);
    public GroupResource Group_ToResource_WithMembers(Group group) => this.mapper.Map<GroupResource_WithMembers>(group);
    public GroupResource Group_ToResource_WithPosts(Group group) => this.mapper.Map<GroupResource_WithPosts>(group);
    public GroupResource Group_ToResource_WithMembers_AndPosts(Group group) => this.mapper.Map<GroupResource_WithMembers_AndPosts>(group);
}
