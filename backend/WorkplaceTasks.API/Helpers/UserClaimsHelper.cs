using System.Security.Claims;

namespace WorkplaceTasks.API.Helpers;

/// <summary>
/// Helper class responsible for extracting user information from JWT claims.
/// Centralizes claim access logic to avoid duplication (DRY).
/// </summary>
public static class UserClaimsHelper
{
    /// <summary>
    /// Returns the authenticated user's Id from the JWT.
    /// </summary>
    public static Guid GetUserId(ClaimsPrincipal user)
    {
        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(id!);
    }

    /// <summary>
    /// Returns the authenticated user's role from the JWT.
    /// Used to apply RBAC rules.
    /// </summary>
    public static string GetUserRole(ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Role)?.Value!;
    }
}
