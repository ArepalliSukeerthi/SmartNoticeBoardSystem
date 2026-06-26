using Microsoft.AspNetCore.Mvc;
using SmartNoticeBoardSystem.Data;
using SmartNoticeBoardSystem.Models;

namespace SmartNoticeBoardSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            DashboardViewModel model = new DashboardViewModel
            {
                TotalNotices = _context.Notices.Count(),

                TotalStudents = _context.Users.Count(x => x.Role == "Student"),

                TotalFaculty = _context.Users.Count(x => x.Role == "Faculty"),

                TotalAdmins = _context.Users.Count(x => x.Role == "Admin"),

                ActiveNotices = _context.Notices.Count(x => x.ExpiryDate >= DateTime.Today),

                ExpiredNotices = _context.Notices.Count(x => x.ExpiryDate < DateTime.Today),

                PinnedNotices = _context.Notices.Count(x => x.IsPinned),

                TotalDownloads = _context.Notices.Sum(x => x.DownloadCount),

                TotalLikes = _context.Notices.Sum(x => x.Likes),

                RecentNotices = _context.Notices
                    .OrderByDescending(x => x.PublishDate)
                    .Take(5)
                    .ToList(),

                MostLikedNotice = _context.Notices
                    .OrderByDescending(x => x.Likes)
                    .FirstOrDefault(),

                MostDownloadedNotice = _context.Notices
                    .OrderByDescending(x => x.DownloadCount)
                    .FirstOrDefault(),

                ExpiringSoon = _context.Notices
                    .Where(x => x.ExpiryDate >= DateTime.Today)
                    .OrderBy(x => x.ExpiryDate)
                    .Take(5)
                    .ToList(),

                PinnedNoticeList = _context.Notices
                    .Where(x => x.IsPinned)
                    .OrderByDescending(x => x.PublishDate)
                    .ToList()
            };

            return View(model);
        }
    }
}