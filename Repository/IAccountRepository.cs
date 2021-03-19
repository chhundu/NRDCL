using Microsoft.AspNetCore.Identity;
using NRDCL.Models.Acc;
using System.Threading.Tasks;

namespace NRDCL.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(SignupUserModel userModel);
        Task<SignInResult> PasswordSignInAsync(SigninUserModel signinModel);
        Task SignOutAsync();
    }
}