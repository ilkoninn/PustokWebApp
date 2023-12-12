using Azure.Core;
using BB205_Pronia.Helpers;
using Microsoft.AspNetCore.Identity;
using WebAppRelation.Enums;

namespace WebAppRelation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {

            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            AppUser appUser = new AppUser()
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
            };

            var resultEmail = await _userManager.FindByEmailAsync(registerVM.Email);

            if (resultEmail == null)
            {
                var result = await _userManager.CreateAsync(appUser, registerVM.Password);
                await _userManager.AddToRoleAsync(appUser, "Admin");

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(registerVM);
                }

                return RedirectToAction(nameof(Login));
            }
            else
            {
                ModelState.AddModelError("Email", "This email used before, please try another email!");
                return View();
            }
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
                
                if(user == null)
                {
                    ModelState.AddModelError("", "Username/Email or password is not valid!");
                    return View();
                }

            }

            if (user != null)
            {
                var result = _signInManager.CheckPasswordSignInAsync(user, loginVM.Password, true).Result;

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Username/Email or password is not valid!");
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Please, try again later!");
                }

                await _signInManager.SignInAsync(user, loginVM.RememberMe);
                if(returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Home", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Username/Email or password is not valid!");
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> CreateRole()
        {
            //string[] userRoles = { "Admin", "Member", "Moderator"};

            //foreach (var userRole in userRoles)
            //{
            //    var roleExist = await _roleManager.RoleExistsAsync(userRole);
            //    if (!roleExist)
            //    {
            //        await _roleManager.CreateAsync(new IdentityRole()
            //        {
            //            Name = userRole
            //        });
            //    }
            //}

            foreach (var item in Enum.GetValues(typeof(UserRole)))
            {
                var roleExist = await _roleManager.RoleExistsAsync(item.ToString());
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = item.ToString()
                    });
                }
            }

            return RedirectToAction("Home", "Home");
        }
    }
}
