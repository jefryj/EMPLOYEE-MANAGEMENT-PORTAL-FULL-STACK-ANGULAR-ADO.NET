using System.Security.Claims;
using EmployeeAdminPortal.Api.Services.Interfaces;
namespace EmployeeAdminPortal.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public int UserId
    {
        get
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(userId, out var id) ? id : 0;
        }
    }

    public string UserName
    {
        get
        {
            return httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        }
    }

    public int RoleId
    {
        get
        {
            var roleId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;

            return int.TryParse(roleId, out var id) ? id : 0;
        }
    }
}