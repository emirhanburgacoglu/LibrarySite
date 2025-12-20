using LibrarySite.Core.Services;
using LibrarySite.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySite.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBooksController : Controller
    {
        private readonly BookService _bookService;
        private readonly LoanService _loanService;
        private readonly ReservationService _reservationService;

        public AdminBooksController(BookService bookService, LoanService loanService, ReservationService reservationService)
        {
            _bookService = bookService;
            _loanService = loanService;
            _reservationService = reservationService;
        }
    

        public IActionResult Index()
        {
            var books = _bookService.GetAll();
            return View(books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new AdminBookCreateVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AdminBookCreateVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var (ok, message) = _bookService.AddBook(vm.Title, vm.Author);

            if (!ok)
            {
                ModelState.AddModelError("", message);
                return View(vm);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
public IActionResult Edit(int id)
{
    var book = _bookService.GetById(id);
    if (book == null) return NotFound();

    var vm = new AdminBookEditVm
    {
        BookId = book.BookId,
        Title = book.Title,
        Author = book.Author,
        IsAvailable = book.IsAvailable
    };

    return View(vm);
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Edit(AdminBookEditVm vm)
{
    if (!ModelState.IsValid)
        return View(vm);

    var (ok, message) = _bookService.UpdateBook(vm.BookId, vm.Title, vm.Author, vm.IsAvailable);

    if (!ok)
    {
        ModelState.AddModelError("", message);
        return View(vm);
    }

    TempData["Success"] = message;
    return RedirectToAction(nameof(Index));
}
[HttpGet]
public IActionResult Delete(int id)
{
    var book = _bookService.GetById(id);
    if (book == null) return NotFound();

    return View(book);
}
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult DeleteConfirmed(int bookId)
{
    // İş kuralı: ödünçte olan kitap silinemez
    if (_loanService.IsBookBorrowed(bookId))
    {
        TempData["Error"] = "This book is currently borrowed. You cannot delete it.";
        return RedirectToAction(nameof(Index));
    }

    // İş kuralı: aktif rezervasyonu olan kitap silinemez
    var hasActiveReservation = _reservationService.GetAll()
        .Any(r => r.BookId == bookId &&
                  (r.Status == LibrarySite.Core.Domain.ReservationStatus.Pending ||
                   r.Status == LibrarySite.Core.Domain.ReservationStatus.Approved));

    if (hasActiveReservation)
    {
        TempData["Error"] = "This book has an active reservation. You cannot delete it.";
        return RedirectToAction(nameof(Index));
    }

    var (ok, message) = _bookService.DeleteBook(bookId);

    TempData[ok ? "Success" : "Error"] = message;
    return RedirectToAction(nameof(Index));
}


    }
}
