public interface ICurrentUserService
{
    int UserId { get; }

    string UserName { get; }

    int RoleId { get; }
}
