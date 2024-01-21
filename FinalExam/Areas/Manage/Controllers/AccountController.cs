using FinalExam.Areas.Manage.ViewModels.Account;
using FinalExam.DAL;
using FinalExam.Models;
using FinalExam.Utilities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalExam.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _usermanager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _rolemanager;

        public AccountController(AppDbContext context, UserManager<AppUser> usermanager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> rolemanager)
        {
            _context = context;
            _usermanager = usermanager;
            _signInManager = signInManager;
            _rolemanager = rolemanager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if(!ModelState.IsValid) return View(loginVM);

            AppUser user = await _usermanager.FindByNameAsync(loginVM.UsernameOrEmail);

            if(user == null)
            {
                user = await _usermanager.FindByEmailAsync(loginVM.UsernameOrEmail);
                if(user == null)
                {
                    ModelState.AddModelError("", "Username or password not correct");
                    return View();
                }
            }

            var results = _signInManager.CheckPasswordSignInAsync(user, loginVM.Password, true).Result;

            if(results.IsLockedOut)
            {
                ModelState.AddModelError("", "Please, try again later");
                return View();
            }

            if(!results.Succeeded)
            {
                ModelState.AddModelError("", "Username or password not correct");
                return View();
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction(nameof(Index), "Home", new { area = "" });
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if(!ModelState.IsValid) return View(registerVM);

            AppUser user = new AppUser()
            {
                Email = registerVM.Email,
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
            };

            
            await _usermanager.CreateAsync(user,registerVM.Password);
            await _usermanager.AddToRoleAsync(user,UserRole.Admin.ToString());

            await _signInManager.SignInAsync(user, false);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Home", new { area = "" });
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home", new { area = "" });
        }

        public async Task<IActionResult> CreateRole()
        {
            foreach (var role in Enum.GetValues(typeof(UserRole)))
            {
                if (!await _rolemanager.RoleExistsAsync(role.ToString()))
                {
                    await _rolemanager.CreateAsync(new IdentityRole()
                    {
                        Name = role.ToString(),
                    });
                }
            }

            return RedirectToAction(nameof(Index), "Home", new { area = "" });
        }


    }
}
