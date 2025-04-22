using CommunitySkillsExchange.Data;
using CommunitySkillsExchange.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommunitySkillsExchange.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Profile/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new ProfileViewModel
            {
                User = user,

                SkillOffers = await _context.SkillOffers
                    .Include(s => s.Category)
                    .Where(s => s.UserId == id && s.IsActive)
                    .OrderByDescending(s => s.CreatedAt)
                    .ToListAsync(),

                SkillRequests = await _context.SkillRequests
                    .Include(s => s.Category)
                    .Where(s => s.UserId == id && s.IsActive)
                    .OrderByDescending(s => s.CreatedAt)
                    .ToListAsync(),

                Reviews = await _context.Reviews
                    .Include(r => r.Reviewer)
                    .Where(r => r.ReviewedUserId == id)
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        // GET: Profile/Edit
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        // POST: Profile/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit([Bind("FirstName,LastName,Bio,ProfilePictureUrl")] ApplicationUser model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Bio = model.Bio;
            user.ProfilePictureUrl = model.ProfilePictureUrl;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Details), new { id = user.Id });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // GET: Profile/AddReview/5
        [Authorize]
        public async Task<IActionResult> AddReview(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (id == currentUser.Id)
            {
                return BadRequest("You cannot review yourself");
            }

            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.ReviewerId == currentUser.Id && r.ReviewedUserId == id);

            var viewModel = new ReviewViewModel
            {
                ReviewedUserId = id,
                ReviewedUserName = $"{user.FirstName} {user.LastName}",
                Rating = existingReview?.Rating ?? 5,
                Comment = existingReview?.Comment ?? ""
            };

            return View(viewModel);
        }

        // POST: Profile/AddReview
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddReview(ReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (model.ReviewedUserId == currentUser.Id)
            {
                return BadRequest("You cannot review yourself");
            }

            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.ReviewerId == currentUser.Id && r.ReviewedUserId == model.ReviewedUserId);

            if (existingReview != null)
            {
                // Update existing review
                existingReview.Rating = model.Rating;
                existingReview.Comment = model.Comment;
                existingReview.CreatedAt = DateTime.Now;

                _context.Update(existingReview);
            }
            else
            {
                // Create new review
                var review = new Review
                {
                    ReviewerId = currentUser.Id,
                    ReviewedUserId = model.ReviewedUserId,
                    Rating = model.Rating,
                    Comment = model.Comment,
                    CreatedAt = DateTime.Now
                };

                _context.Add(review);
            }

            await _context.SaveChangesAsync();

            // Update the user's average rating
            await UpdateUserRating(model.ReviewedUserId);

            return RedirectToAction(nameof(Details), new { id = model.ReviewedUserId });
        }

        private async Task UpdateUserRating(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var reviews = await _context.Reviews.Where(r => r.ReviewedUserId == userId).ToListAsync();

            if (reviews.Any())
            {
                user.Rating = (decimal)reviews.Average(r => r.Rating);
                user.ReviewCount = reviews.Count;
                await _userManager.UpdateAsync(user);
            }
        }
    }
}