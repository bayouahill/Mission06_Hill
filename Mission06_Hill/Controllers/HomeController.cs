using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mission06_Hill.Models;

namespace Mission06_Hill.Controllers
{
    public class HomeController : Controller
    {
        private readonly MovieFormContext _context;

        public HomeController(MovieFormContext newMovie)
        {
            _context = newMovie;
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
                CategoryOptions = new SelectList(_context.Categories, "CategoryId", "CategoryName"),
                RatingOptions = new SelectList(_context.Ratings, "RatingId", "RatingName")
            };

            ViewBag.Directors = _context.Directors.OrderBy(d => d.DirectorName).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MovieForm(MovieEntryForm formData)
        {
            if (ModelState.IsValid)
            {
                int directorId = FindOrCreateDirector(formData.DirectorName.Trim());

                var movie = new Movie
                {
                    Title = formData.Title,
                    Year = formData.Year,
                    Edited = formData.Edited,
                    LentTo = formData.LentTo,
                    Notes = formData.Notes,
                    CategoryId = formData.CategoryId,
                    DirectorId = directorId,
                    RatingId = formData.RatingId
                };
                _context.Movies.Add(movie);
                _context.SaveChanges();
                TempData["Success"] = $"\"{formData.Title}\" added to the archive!";
                return RedirectToAction("MovieList");
            }

            // Reload dropdowns if validation fails
            formData.CategoryOptions = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            formData.RatingOptions = new SelectList(_context.Ratings, "RatingId", "RatingName");
            ViewBag.Directors = _context.Directors.OrderBy(d => d.DirectorName).ToList();

            return View(formData);
        }

        // ── LIST MOVIES ──────────────────────────────────────────────────────────

        public IActionResult MovieList()
        {
            var movies = _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Director)
                .Include(m => m.Rating)
                .OrderBy(m => m.Title)
                .ToList();

            return View(movies);
        }

        // ── EDIT MOVIE ───────────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult EditMovie(int id)
        {
            var movie = _context.Movies
                .Include(m => m.Director)
                .FirstOrDefault(m => m.MovieId == id);

            if (movie == null) return NotFound();

            var model = new MovieEntryForm
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Year = movie.Year,
                Edited = movie.Edited,
                LentTo = movie.LentTo,
                Notes = movie.Notes,
                CategoryId = movie.CategoryId,
                RatingId = movie.RatingId,
                DirectorName = movie.Director?.DirectorName ?? string.Empty,
                CategoryOptions = new SelectList(_context.Categories, "CategoryId", "CategoryName", movie.CategoryId),
                RatingOptions = new SelectList(_context.Ratings, "RatingId", "RatingName", movie.RatingId)
            };

            ViewBag.Directors = _context.Directors.OrderBy(d => d.DirectorName).ToList();

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

                int directorId = FindOrCreateDirector(formData.DirectorName.Trim());

                movie.Title = formData.Title;
                movie.Year = formData.Year;
                movie.Edited = formData.Edited;
                movie.LentTo = formData.LentTo;
                movie.Notes = formData.Notes;
                movie.CategoryId = formData.CategoryId;
                movie.RatingId = formData.RatingId;
                movie.DirectorId = directorId;

                _context.SaveChanges();
                TempData["Success"] = $"\"{formData.Title}\" updated successfully!";
                return RedirectToAction("MovieList");
            }

            // Reload dropdowns if validation fails
            formData.CategoryOptions = new SelectList(_context.Categories, "CategoryId", "CategoryName", formData.CategoryId);
            formData.RatingOptions = new SelectList(_context.Ratings, "RatingId", "RatingName", formData.RatingId);
            ViewBag.Directors = _context.Directors.OrderBy(d => d.DirectorName).ToList();

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

        private int FindOrCreateDirector(string directorName)
        {
            var existing = _context.Directors
                .FirstOrDefault(d => d.DirectorName.ToLower() == directorName.ToLower());

            if (existing != null)
                return existing.DirectorId;

            var newDirector = new Director { DirectorName = directorName };
            _context.Directors.Add(newDirector);
            _context.SaveChanges();
            return newDirector.DirectorId;
        }
    }
}
