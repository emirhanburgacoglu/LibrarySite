using LibrarySite.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySite.Web.Controllers
{
    [Authorize] // login olmayan göremez
    public class BooksController : Controller
    {
        private readonly BookService _bookService;
        private readonly LoanService _loanService;

        // ✅ DOĞRU CONSTRUCTOR (DI ile)
        public BooksController(BookService bookService, LoanService loanService)
        {
            _bookService = bookService;
            _loanService = loanService;
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

            // Loan varsa kitap available değil
            if (_loanService.IsBookBorrowed(book.BookId))
            {
                book.IsAvailable = false;
            }

            return View(book);
        }
    }
}
