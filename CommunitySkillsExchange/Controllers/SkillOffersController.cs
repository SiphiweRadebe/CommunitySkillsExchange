using CommunitySkillsExchange.Data;
using CommunitySkillsExchange.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CommunitySkillsExchange.Controllers
{
    public class SkillOffersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SkillOffersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SkillOffers
        public async Task<IActionResult> Index(string searchString, int? categoryId)
        {
            var offers = _context.SkillOffers
                .Include(s => s.Category)
                .Include(s => s.User)
                .Where(s => s.IsActive);

            ViewData["Categories"] = new SelectList(_context.SkillCategories, "Id", "Name");

            if (!string.IsNullOrEmpty(searchString))
            {
                offers = offers.Where(s => s.Title.Contains(searchString) ||
                                          s.Description.Contains(searchString) ||
                                          s.LocationDescription.Contains(searchString));
                ViewData["SearchString"] = searchString;
            }

            if (categoryId.HasValue)
            {
                offers = offers.Where(s => s.CategoryId == categoryId);
                ViewData["CategoryId"] = categoryId;
            }

            return View(await offers.OrderByDescending(s => s.CreatedAt).ToListAsync());
        }

        // GET: SkillOffers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skillOffer = await _context.SkillOffers
                .Include(s => s.Category)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (skillOffer == null)
            {
                return NotFound();
            }

            return View(skillOffer);
        }

        // GET: SkillOffers/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.SkillCategories, "Id", "Name");
            return View();
        }

        // POST: SkillOffers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Title,Description,CategoryId,Latitude,Longitude,LocationDescription")] SkillOffer skillOffer)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                skillOffer.UserId = user.Id;
                skillOffer.CreatedAt = DateTime.Now;
                skillOffer.UpdatedAt = DateTime.Now;

                _context.Add(skillOffer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.SkillCategories, "Id", "Name", skillOffer.CategoryId);
            return View(skillOffer);
        }

        // GET: SkillOffers/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skillOffer = await _context.SkillOffers.FindAsync(id);
            if (skillOffer == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (skillOffer.UserId != user.Id && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            ViewData["CategoryId"] = new SelectList(_context.SkillCategories, "Id", "Name", skillOffer.CategoryId);
            return View(skillOffer);
        }

        // POST: SkillOffers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CategoryId,IsActive,Latitude,Longitude,LocationDescription")] SkillOffer skillOffer)
        {
            if (id != skillOffer.Id)
            {
                return NotFound();
            }

            var existingOffer = await _context.SkillOffers.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);

            if (existingOffer == null || (existingOffer.UserId != user.Id && !User.IsInRole("Admin")))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingOffer.Title = skillOffer.Title;
                    existingOffer.Description = skillOffer.Description;
                    existingOffer.CategoryId = skillOffer.CategoryId;
                    existingOffer.IsActive = skillOffer.IsActive;
                    existingOffer.Latitude = skillOffer.Latitude;
                    existingOffer.Longitude = skillOffer.Longitude;
                    existingOffer.LocationDescription = skillOffer.LocationDescription;
                    existingOffer.UpdatedAt = DateTime.Now;

                    _context.Update(existingOffer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkillOfferExists(skillOffer.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.SkillCategories, "Id", "Name", skillOffer.CategoryId);
            return View(skillOffer);
        }

        // GET: SkillOffers/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skillOffer = await _context.SkillOffers
                .Include(s => s.Category)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (skillOffer == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (skillOffer.UserId != user.Id && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return View(skillOffer);
        }

        // POST: SkillOffers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var skillOffer = await _context.SkillOffers.FindAsync(id);

            if (skillOffer == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (skillOffer.UserId != user.Id && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            _context.SkillOffers.Remove(skillOffer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SkillOfferExists(int id)
        {
            return _context.SkillOffers.Any(e => e.Id == id);
        }
    }
}