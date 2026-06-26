using Microsoft.AspNetCore.Mvc;
using SmartNoticeBoardSystem.Data;
using SmartNoticeBoardSystem.Models;

namespace SmartNoticeBoardSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(x =>
                x.Email == email &&
                x.PasswordHash == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid Email or Password";
                return View();
            }

            HttpContext.Session.SetString("Role", user.Role!);
            HttpContext.Session.SetString("Name", user.FullName!);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
{
    return View();
}

[HttpPost]
[HttpPost]
public IActionResult Register(User user)
{
    user.CreatedDate = DateTime.Now;

    _context.Users.Add(user);

    _context.SaveChanges();

    return RedirectToAction("Login");
}
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}