using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using YourLocalShop.Models;
using YourLocalShop.Models.ViewModels;

namespace YourLocalShop.Controllers
{
    public class AccountController(UsersRepository users) : Controller
    {
        // GET: /Account/LoginOnly
        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoginOnly() => View("LoginRegister", new AuthPageVm { ActiveTab = "login" });

        // GET: /Account/Account
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Account()
        {
            if (User.Identity?.IsAuthenticated != true)
                return View("LoginRegister", new AuthPageVm { ActiveTab = "login" });
            var email = User.FindFirstValue(ClaimTypes.Email);
            var u = users.FindByEmail(email);

            if (u != null)
                return User.IsInRole("Employee")
                    ? RedirectToAction("Dashboard", "Employee")
                    : RedirectToAction(nameof(Settings));
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(LoginOnly));
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind(Prefix = "Login")] LoginVm vm)
        {
            if (!ModelState.IsValid)
                return View("LoginRegister", new AuthPageVm { Login = vm, ActiveTab = "login" });

            var user = users.FindByEmail(vm.Email);
            var ok = user != null &&
                     users.Hasher.VerifyHashedPassword(user, user.PasswordHash, vm.Password)
                     != PasswordVerificationResult.Failed;

            if (!ok)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View("LoginRegister", new AuthPageVm { Login = vm, ActiveTab = "login" });
            }

            await SignInAsync(user!);
            return user!.Role == "Employee"
                ? RedirectToAction("Dashboard", "Employee")
                : RedirectToAction(nameof(Settings));
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind(Prefix = "Register")] RegisterVm vm)
        {
            if (!ModelState.IsValid)
                return View("LoginRegister", new AuthPageVm { Register = vm, ActiveTab = "register" });

            if (users.FindByEmail(vm.Email) != null)
            {
                ModelState.AddModelError(nameof(vm.Email), "Email already registered.");
                return View("LoginRegister", new AuthPageVm { Register = vm, ActiveTab = "register" });
            }

            var customer = new Customer
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                Phone = vm.Phone,
                Address = new Address
                {
                    StreetNumber = vm.StreetNumber,
                    StreetName = vm.StreetName,
                    City = vm.City,
                    State = vm.State,
                    Country = vm.Country,
                    PostCode = vm.PostCode
                }
            };
            customer.PasswordHash = users.Hasher.HashPassword(customer, vm.Password);
            users.Add(customer);

            await SignInAsync(customer);
            return RedirectToAction(nameof(Settings));
        }

        // POST: /Account/Logout
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // wipe all cookies while testing
            foreach (var k in Request.Cookies.Keys)
                Response.Cookies.Delete(k);

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Settings
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Settings()
        {
            if (User.IsInRole("Employee"))
                return RedirectToAction("Dashboard", "Employee");

            var email = User.FindFirstValue(ClaimTypes.Email);
            var customer = users.FindByEmail(email) as Customer;

            if (customer == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction(nameof(LoginOnly));
            }

            var vm = new AccountVm
            {
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
                StreetNumber = customer.Address.StreetNumber,
                StreetName = customer.Address.StreetName,
                City = customer.Address.City,
                State = customer.Address.State,
                Country = customer.Address.Country,
                PostCode = customer.Address.PostCode
            };
            return View(vm);
        }

        // POST: /Account/Update
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AccountVm vm)
        {
            if (!ModelState.IsValid) return View("Settings", vm);

            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = users.FindByEmail(email) as Customer;
            if (user == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction(nameof(LoginOnly));
            }

            // apply updates
            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            user.Phone = vm.Phone;

            user.Address.StreetNumber = vm.StreetNumber;
            user.Address.StreetName = vm.StreetName;
            user.Address.City = vm.City;
            user.Address.State = vm.State;
            user.Address.Country = vm.Country;
            user.Address.PostCode = vm.PostCode;

            users.Update(user);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await SignInAsync(user);

            ViewBag.Saved = true;
            return View("Settings", vm);
        }
        
        // POST: /Account/CheckEmail
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult CheckEmail(string email)
        {
            var exists = users.FindByEmail(email) != null;
            return Json(new { exists });
        }

        // POST: /Account/ChangePassword
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordVm vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["pwd_error"] = "Please fix the errors and try again.";
                return RedirectToAction(nameof(Settings));
            }

            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = users.FindByEmail(email);
            if (user == null)
            {
                TempData["pwd_error"] = "User not found.";
                return RedirectToAction(nameof(Account));
            }

            // verify old password
            var ok = users.Hasher.VerifyHashedPassword(user, user.PasswordHash, vm.OldPassword)
                     != PasswordVerificationResult.Failed;
            if (!ok)
            {
                TempData["pwd_error"] = "Old password is incorrect.";
                return RedirectToAction(nameof(Settings));
            }

            // set new password
            user.PasswordHash = users.Hasher.HashPassword(user, vm.NewPassword);
            users.Update(user);

            TempData["pwd_ok"] = "Password changed successfully.";
            return RedirectToAction(nameof(Settings));
        }

        // POST: /Account/ValidateOldPassword
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult ValidateOldPassword(string oldPassword)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = users.FindByEmail(email);
            if (user == null) return Json(new { ok = false, message = "User not found." });

            var ok = users.Hasher.VerifyHashedPassword(user, user.PasswordHash, oldPassword)
                     != PasswordVerificationResult.Failed;

            return ok
                ? Json(new { ok = true })
                : Json(new { ok = false, message = "Old password is incorrect." });
        }

        // helper to sign in a user
        private async Task SignInAsync(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new(ClaimTypes.Role, user.Role)
            };

            var principal = new ClaimsPrincipal(
                new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}