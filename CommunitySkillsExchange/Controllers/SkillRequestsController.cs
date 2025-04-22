using CommunitySkillsExchange.Data;
using CommunitySkillsExchange.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CommunitySkillsExchange.Controllers
{
    public class SkillRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SkillRequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SkillRequests
        public async Task<IActionResult> Index(string searchString, int? categoryId)
        {
            var requests = _context.SkillRequests
                .Include(s => s.Category)
                .Include(s => s.User)
                .Where(s => s.IsActive);

            ViewData["Categories"] = new SelectList(_context.SkillCategories, "Id", "Name");

            if (!string.IsNullOrEmpty(searchString))
            {
                requests = requests.Where(s => s.Title.Contains(searchString) ||
                                          s.Description.Contains(searchString) ||
                                          s.LocationDescription.Contains(searchString));
                ViewData["SearchString"] = searchString;
            }

            if (categoryId.HasValue)
            {
                requests = requests.Where(s => s.CategoryId == categoryId);
                ViewData["CategoryId"] = categoryId;
            }

            return View(await requests.OrderByDescending(s => s.CreatedAt).ToListAsync());
        }

        // GET: SkillRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skillRequest = await _context.SkillRequests
                .Include(s => s.Category)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (skillRequest == null)
            {
                return NotFound();
            }

            return View(skillRequest);
        }

        // GET: SkillRequests/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.SkillCategories, "Id", "Name");
            return View();
        }

        // POST: SkillRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Title,Description,CategoryId,Latitude,Longitude,LocationDescription")] SkillRequest skillRequest)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                skillRequest.UserId = user.Id;
                skillRequest.CreatedAt = DateTime.Now;
                skillRequest.UpdatedAt = DateTime.Now;

                _context.Add(skillRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.SkillCategories, "Id", "Name", skillRequest.CategoryId);
            return View(skillRequest);
        }

        // GET: SkillRequests/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skillRequest = await _context.SkillRequests.FindAsync(id);
            if (skillRequest == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (skillRequest.UserId != user.Id && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            ViewData["CategoryId"] = new SelectList(_context.SkillCategories, "Id", "Name", skillRequest.CategoryId);
            return View(skillRequest);
        }

        // POST: SkillRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CategoryId,IsActive,Latitude,Longitude,LocationDescription")] SkillRequest skillRequest)
        {
            if (id != skillRequest.Id)
            {
                return NotFound();
            }

            var existingRequest = await _context.SkillRequests.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);

            if (existingRequest == null || (existingRequest.UserId != user.Id && !User.IsInRole("Admin")))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingRequest.Title = skillRequest.Title;
                    existingRequest.Description = skillRequest.Description;
                    existingRequest.CategoryId = skillRequest.CategoryId;
                    existingRequest.IsActive = skillRequest.IsActive;
                    existingRequest.Latitude = skillRequest.Latitude;
                    existingRequest.Longitude = skillRequest.Longitude;
                    existingRequest.LocationDescription = skillRequest.LocationDescription;
                    existingRequest.UpdatedAt = DateTime.Now;

                    _context.Update(existingRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkillRequestExists(skillRequest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.SkillCategories, "Id", "Name", skillRequest.CategoryId);
            return View(skillRequest);
        }

        // GET: SkillRequests/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skillRequest = await _context.SkillRequests
                .Include(s => s.Category)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (skillRequest == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (skillRequest.UserId != user.Id && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return View(skillRequest);
        }

        // POST: SkillRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var skillRequest = await _context.SkillRequests.FindAsync(id);

            if (skillRequest == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (skillRequest.UserId != user.Id && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            _context.SkillRequests.Remove(skillRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SkillRequestExists(int id)
        {
            return _context.SkillRequests.Any(e => e.Id == id);
        }
    }
}