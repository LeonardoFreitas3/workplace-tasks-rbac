using System.Security.Claims;

namespace WorkplaceTasks.API.Helpers;

public static class UserClaimsHelper
{
    public static Guid GetUserId(ClaimsPrincipal user)
    {
        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(id!);
    }

    public static string GetUserRole(ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Role)?.Value!;
    }
}
