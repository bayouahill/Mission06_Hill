using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mission06_Hill.Models;

namespace Mission06_Hill.Controllers
{
    public class HomeController : Controller
    {
        private readonly MovieFormContext _context;

        // Ratings are a fixed set — stored as plain text in the Movies table
        private static readonly string[] KnownRatings =
            { "G", "PG", "PG-13", "R", "NR", "UR", "TV-G", "TV-PG", "TV-14", "TV-Y7" };

        public HomeController(MovieFormContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetToKnowJoel()
        {
            return View();
        }

        // ── ADD MOVIE ────────────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult MovieForm()
        {
            var model = new MovieEntryForm
            {
                CategoryOptions = new SelectList(
                    _context.Categories.OrderBy(c => c.CategoryName), "CategoryId", "CategoryName")
            };

            LoadFormViewBag();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MovieForm(MovieEntryForm formData)
        {
            if (ModelState.IsValid)
            {
                var movie = new Movie
                {
                    CategoryId   = formData.CategoryId,
                    Title        = formData.Title,
                    Year         = formData.Year,
                    Director     = formData.Director?.Trim(),
                    Rating       = formData.Rating,
                    Edited       = formData.Edited,
                    LentTo       = formData.LentTo,
                    CopiedToPlex = formData.CopiedToPlex,
                    Notes        = formData.Notes
                };
                _context.Movies.Add(movie);
                _context.SaveChanges();
                TempData["Success"] = $"\"{formData.Title}\" added to the archive!";
                return RedirectToAction("MovieList");
            }

            // Reload dropdowns if validation fails
            formData.CategoryOptions = new SelectList(
                _context.Categories.OrderBy(c => c.CategoryName), "CategoryId", "CategoryName");
            LoadFormViewBag(formData.Rating);
            return View(formData);
        }

        // ── LIST MOVIES ──────────────────────────────────────────────────────────

        public IActionResult MovieList()
        {
            var movies = _context.Movies
                .Include(m => m.Category)
                .OrderBy(m => m.Title)
                .ToList();

            return View(movies);
        }

        // ── EDIT MOVIE ───────────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult EditMovie(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null) return NotFound();

            var model = new MovieEntryForm
            {
                MovieId      = movie.MovieId,
                CategoryId   = movie.CategoryId,
                Title        = movie.Title,
                Year         = movie.Year,
                Director     = movie.Director,
                Rating       = movie.Rating,
                Edited       = movie.Edited,
                LentTo       = movie.LentTo,
                CopiedToPlex = movie.CopiedToPlex,
                Notes        = movie.Notes,
                CategoryOptions = new SelectList(
                    _context.Categories.OrderBy(c => c.CategoryName),
                    "CategoryId", "CategoryName", movie.CategoryId)
            };

            LoadFormViewBag(movie.Rating);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMovie(int id, MovieEntryForm formData)
        {
            if (ModelState.IsValid)
            {
                var movie = _context.Movies.Find(id);
                if (movie == null) return NotFound();

                movie.CategoryId   = formData.CategoryId;
                movie.Title        = formData.Title;
                movie.Year         = formData.Year;
                movie.Director     = formData.Director?.Trim();
                movie.Rating       = formData.Rating;
                movie.Edited       = formData.Edited;
                movie.LentTo       = formData.LentTo;
                movie.CopiedToPlex = formData.CopiedToPlex;
                movie.Notes        = formData.Notes;

                _context.SaveChanges();
                TempData["Success"] = $"\"{formData.Title}\" updated successfully!";
                return RedirectToAction("MovieList");
            }

            // Reload dropdowns if validation fails
            formData.CategoryOptions = new SelectList(
                _context.Categories.OrderBy(c => c.CategoryName),
                "CategoryId", "CategoryName", formData.CategoryId);
            LoadFormViewBag(formData.Rating);
            return View(formData);
        }

        // ── DELETE MOVIE ─────────────────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMovie(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
                TempData["Success"] = $"\"{movie.Title}\" removed from the archive.";
            }
            return RedirectToAction("MovieList");
        }

        // ── HELPER ───────────────────────────────────────────────────────────────

        /// <summary>
        /// Populates ViewBag.Directors (autocomplete) and ViewBag.Ratings (select list).
        /// </summary>
        private void LoadFormViewBag(string? selectedRating = null)
        {
            ViewBag.Directors = _context.Movies
                .Where(m => m.Director != null)
                .Select(m => m.Director!)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            ViewBag.Ratings = new SelectList(KnownRatings, selectedRating);
        }
    }
}
