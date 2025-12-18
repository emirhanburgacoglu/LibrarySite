using LibrarySite.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySite.Web.Controllers
{
    [Authorize] // login olmayan göremez
    public class BooksController : Controller
    {
        private readonly BookService _bookService;

        public BooksController()
        {
            _bookService = new BookService();
        }

        public IActionResult Index()
        {
            var books = _bookService.GetAll();
            return View(books);
        }

        public IActionResult Details(int id)
        {
            var book = _bookService.GetById(id);
            if (book == null)
                return NotFound();

            return View(book);
        }
    }
}