using Microsoft.AspNetCore.Http;
using NRDCL.IService.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NRDCL.Service.User
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContext;

        public UserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        public string GetUserId()
        {
            return _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
