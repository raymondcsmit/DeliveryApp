using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeliveryApi.Controllers
{
    public class BaseController : ControllerBase
    {
        [NonAction]
        public int GetUserID()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return 0;
            }

            int userId = int.Parse(userIdClaim);
            return userId;
        }
    }
}
