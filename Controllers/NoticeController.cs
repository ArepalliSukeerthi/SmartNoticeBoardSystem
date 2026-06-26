using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartNoticeBoardSystem.Data;
using SmartNoticeBoardSystem.Models;

namespace SmartNoticeBoardSystem.Controllers
{
    public class NoticeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public NoticeController(ApplicationDbContext context,
                                IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // ---------------------------
        // Helper Methods
        // ---------------------------

        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetString("Role") != null;
        }

        private bool IsAdminOrFaculty()
        {
            var role = HttpContext.Session.GetString("Role");

            return role == "Admin" || role == "Faculty";
        }

        // ---------------------------
        // Notice List
        // ---------------------------

        public IActionResult Index(string searchString,
                                   string category)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var notices = _context.Notices.AsQueryable();

            // Search

            if (!string.IsNullOrEmpty(searchString))
            {
                notices = notices.Where(x =>
                    x.Title!.Contains(searchString) ||
                    x.Description!.Contains(searchString));
            }

            // Category Filter

            if (!string.IsNullOrEmpty(category))
            {
                notices = notices.Where(x =>
                    x.Category == category);
            }

            ViewBag.Search = searchString;
            ViewBag.Category = category;

            return View(
                notices
                .OrderByDescending(x => x.PublishDate)
                .ToList());
        }

        // ---------------------------
        // Notice Details
        // ---------------------------

        public IActionResult Details(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var notice =
                _context.Notices
                .FirstOrDefault(x => x.NoticeId == id);

            if (notice == null)
            {
                return NotFound();
            }

            return View(notice);
        }

        // ---------------------------
        // Create Notice
        // ---------------------------

        public IActionResult Create()
        {
            if (!IsAdminOrFaculty())
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            Notice notice,
            IFormFile? uploadFile)
        {
            if (!IsAdminOrFaculty())
            {
                return RedirectToAction(nameof(Index));
            }

            if (uploadFile != null &&
                uploadFile.Length > 0)
            {
                string uploadFolder =
                    Path.Combine(
                        _environment.WebRootPath,
                        "uploads");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                string fileName =
                    Guid.NewGuid().ToString() +
                    Path.GetExtension(uploadFile.FileName);

                string filePath =
                    Path.Combine(uploadFolder,
                                 fileName);

                using (var stream =
                    new FileStream(filePath,
                                   FileMode.Create))
                {
                    await uploadFile.CopyToAsync(stream);
                }

                notice.FilePath =
                    "/uploads/" + fileName;
            }

            notice.PublishDate =
                DateTime.Now;

            _context.Notices.Add(notice);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
                // ---------------------------
        // Edit Notice
        // ---------------------------

        public IActionResult Edit(int id)
        {
            if (!IsAdminOrFaculty())
            {
                return RedirectToAction(nameof(Index));
            }

            var notice = _context.Notices.Find(id);

            if (notice == null)
                return NotFound();

            return View(notice);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Notice notice, IFormFile? uploadFile)
        {
            if (!IsAdminOrFaculty())
            {
                return RedirectToAction(nameof(Index));
            }

            var existing = _context.Notices.Find(notice.NoticeId);

            if (existing == null)
                return NotFound();

            existing.Title = notice.Title;
            existing.Description = notice.Description;
            existing.Category = notice.Category;
            existing.Priority = notice.Priority;
            existing.ExpiryDate = notice.ExpiryDate;
            existing.IsPinned = notice.IsPinned;

            if (uploadFile != null && uploadFile.Length > 0)
            {
                string folder = Path.Combine(_environment.WebRootPath, "uploads");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() +
                                  Path.GetExtension(uploadFile.FileName);

                string path = Path.Combine(folder, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(stream);
                }

                existing.FilePath = "/uploads/" + fileName;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ---------------------------
        // Delete Notice
        // ---------------------------

        public IActionResult Delete(int id)
        {
            if (!IsAdminOrFaculty())
            {
                return RedirectToAction(nameof(Index));
            }

            var notice = _context.Notices.Find(id);

            if (notice != null)
            {
                _context.Notices.Remove(notice);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // ---------------------------
        // Like Notice
        // ---------------------------

        public IActionResult Like(int id)
        {
            var notice = _context.Notices.Find(id);

            if (notice != null)
            {
                notice.Likes++;

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // ---------------------------
        // Download Attachment
        // ---------------------------

        public IActionResult Download(int id)
        {
            var notice = _context.Notices.Find(id);

            if (notice == null || string.IsNullOrEmpty(notice.FilePath))
            {
                return RedirectToAction(nameof(Index));
            }

            notice.DownloadCount++;

            _context.SaveChanges();

            return Redirect(notice.FilePath);
        }

        // ---------------------------
        // Pin Notice
        // ---------------------------

        public IActionResult TogglePin(int id)
        {
            if (!IsAdminOrFaculty())
            {
                return RedirectToAction(nameof(Index));
            }

            var notice = _context.Notices.Find(id);

            if (notice != null)
            {
                notice.IsPinned = !notice.IsPinned;

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}