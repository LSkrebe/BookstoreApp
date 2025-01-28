using BookstoreApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApp.Controllers
{
    public class BookController : Controller
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("api/books")]
        public IActionResult GetBooks(int page = 1, int seed = 0, int avgLikes = 100, double avgReviews = 5.0, string locale = "en")
        {
            try
            {
                int combinedSeed = (int)(seed + page + avgLikes + avgReviews);
                var generator = new BookGenerator(combinedSeed, locale);
                var books = generator.GenerateBooks(page, 20, avgLikes, avgReviews);
                return Ok(books);
            }
            catch (Exception ex)
            {
                // Log error for debugging
                Console.WriteLine($"Error generating books: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
