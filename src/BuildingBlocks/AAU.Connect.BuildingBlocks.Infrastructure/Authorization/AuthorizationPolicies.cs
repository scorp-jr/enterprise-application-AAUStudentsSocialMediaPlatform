namespace AAU.Connect.BuildingBlocks.Infrastructure.Authorization;

public static class AuthorizationPolicies
{
    public const string RequireStudentRole = "RequireStudentRole";
    public const string RequireInstructorRole = "RequireInstructorRole";
    public const string RequireAdminRole = "RequireAdminRole";
}
