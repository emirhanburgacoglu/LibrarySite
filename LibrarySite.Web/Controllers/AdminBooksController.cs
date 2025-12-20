using LibrarySite.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySite.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBooksController : Controller
    {
        private readonly BookService _bookService;

        public AdminBooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        // GET: /AdminBooks
        public IActionResult Index()
        {
            var books = _bookService.GetAll();
            return View(books);
        }
    }
}
