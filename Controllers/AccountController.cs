using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NRDCL.IService.User;
using NRDCL.Models.Acc;
using NRDCL.Models.Common;
using NRDCL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(IAccountRepository accountRepository, UserManager<ApplicationUser> userManager, IUserService userService, RoleManager<IdentityRole> roleManager)
        {
            _userService = userService;
            _roleManager = roleManager;
            _accountRepository = accountRepository;
            _userManager = userManager;
        }
        public IActionResult Signup()
        {
            var roles = _roleManager.Roles;
            ViewData["Roles"] = new SelectList(roles);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupUserModel userModel)
        {
            var roles = _roleManager.Roles;
            ViewData["Roles"] = new SelectList(roles);
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.CreateUserAsync(userModel);
      
                if (!result.Succeeded)
                {
                    foreach (var errorMsg in result.Errors)
                    {
                        ModelState.AddModelError("", errorMsg.Description);
                    }
                    return View(userModel);
                }
                ViewBag.Result = CommonProperties.signupSuccessMsg;
                ModelState.Clear();
                userModel = new SignupUserModel();
            }
            return View(userModel);
        }

        public IActionResult Signin()
        {
            var roles = _roleManager.Roles;
            ViewData["Roles"] = new SelectList(roles);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signin(SigninUserModel signinUserModel, string returnUrl)
        {
            var roles = _roleManager.Roles;
            ViewData["Roles"] = new SelectList(roles);
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.PasswordSignInAsync(signinUserModel);
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
            await _accountRepository.SignOutAsync();
            return RedirectToAction("Signin");
        }
        [Route("change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.ChangePasswordAsync(changePasswordModel);
                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = CommonProperties.updateSuccessMsg;
                    ModelState.Clear();
                    return View();
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(changePasswordModel);
        }
    }
}
