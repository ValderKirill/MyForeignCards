using System.Security.Claims;

namespace MyForeignCards.Utils
{
    public static class UserUtils
    {
        public static bool GetId(ClaimsPrincipal claims, out Guid userId)
        {
            var id = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(id, out userId);

        }
    }
}
