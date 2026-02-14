using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mission06_Hill.Models;
using System;

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

        [HttpGet]
        public IActionResult MovieForm()
        {
            var model = new MovieEntryForm
            {
                CategoryOptions = new SelectList(_context.Categories, "CategoryId", "CategoryName"),
                RatingOptions = new SelectList(_context.Ratings, "RatingId", "RatingName")
            };

            // Pass directors list for autocomplete
            ViewBag.Directors = _context.Directors.OrderBy(d => d.DirectorName).ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult MovieForm(MovieEntryForm formData)
        {
            if (ModelState.IsValid)
            {
                // Find or create director (case-insensitive)
                var directorName = formData.DirectorName.Trim();
                var existingDirector = _context.Directors
                    .FirstOrDefault(d => d.DirectorName.ToLower() == directorName.ToLower());

                int directorId;
                if (existingDirector != null)
                {
                    // Use existing director
                    directorId = existingDirector.DirectorId;
                }
                else
                {
                    // Create new director
                    var newDirector = new Director { DirectorName = directorName };
                    _context.Directors.Add(newDirector);
                    _context.SaveChanges(); // Save to get the new DirectorId
                    directorId = newDirector.DirectorId;
                }

                // Create movie with the director ID
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
                TempData["Success"] = "Movie added successfully!";
                return RedirectToAction("MovieForm");
            }

            // reload dropdowns if validation fails
            formData.CategoryOptions = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            formData.RatingOptions = new SelectList(_context.Ratings, "RatingId", "RatingName");

            // Reload directors for autocomplete
            ViewBag.Directors = _context.Directors.OrderBy(d => d.DirectorName).ToList();

            return View(formData);
        }
    }
}