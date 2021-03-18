using Microsoft.AspNetCore.Mvc;
using NRDCL.Models.Acc;
using NRDCL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }
        [Route("signup")]
        public IActionResult Signup()
        {
            return View();
        }
        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> Signup(SignupUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var result = await this.accountRepository.CreateUserAsync(userModel);
                if (!result.Succeeded)
                {
                    foreach(var errorMsg in result.Errors)
                    {
                        ModelState.AddModelError("", errorMsg.Description);
                    }
                    return View(userModel);
                }
                ModelState.Clear();
            }
            return View();
        }

        [Route("signin")]
        public IActionResult Signin()
        {
            return View();
        }
        [Route("signin")]
        [HttpPost]
        public async Task<IActionResult> Signin(SigninUserModel signinUserModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await this.accountRepository.PasswordSignInAsync(signinUserModel);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid Credentials! Either email or password is invalid.");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await this.accountRepository.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
