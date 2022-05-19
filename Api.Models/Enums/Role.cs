
namespace Api.Models.Enums;

public enum Role
{
    User,
    Moderator,
    Admin,
}

public static class RoleParser
{
    public static string Handle(string value)
    {
        if (EqualsIgnoreCase(value, nameof(Role.User)))
        {
            return nameof(Role.User);
        }
        else if (EqualsIgnoreCase(value, nameof(Role.Moderator)))
        {
            return nameof(Role.Moderator);
        }
        else if (EqualsIgnoreCase(value, nameof(Role.Admin)))
        {
            return nameof(Role.Admin);
        }
        else
        {
            return string.Empty;
        }
    }

    private static bool EqualsIgnoreCase(string val1, string val2)
    {
        return val1.Equals(val2, StringComparison.OrdinalIgnoreCase);
    }
}
